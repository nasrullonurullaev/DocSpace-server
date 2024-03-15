// (c) Copyright Ascensio System SIA 2009-2024
// 
// This program is a free software product.
// You can redistribute it and/or modify it under the terms
// of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
// Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
// to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of
// any third-party rights.
// 
// This program is distributed WITHOUT ANY WARRANTY, without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see
// the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
// 
// You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
// 
// The  interactive user interfaces in modified source and object code versions of the Program must
// display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
// 
// Pursuant to Section 7(b) of the License you must retain the original Product logo when
// distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under
// trademark law for use of our trademarks.
// 
// All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
// content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
// International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode

namespace ASC.Web.Core.Users;

public static class UserPhotoThumbnailManager
{
    public static async Task<List<ThumbnailItem>> SaveThumbnails(UserPhotoManager userPhotoManager, SettingsManager settingsManager, int x, int y, int width, int height, Guid userId)
    {
        return await SaveThumbnails(userPhotoManager, settingsManager, new UserPhotoThumbnailSettings(x, y, width, height), userId);
    }

    public static async Task<List<ThumbnailItem>> SaveThumbnails(UserPhotoManager userPhotoManager, SettingsManager settingsManager, Point point, Size size, Guid userId)
    {
        return await SaveThumbnails(userPhotoManager, settingsManager, new UserPhotoThumbnailSettings(point, size), userId);
    }

    public static async Task<List<ThumbnailItem>> SaveThumbnails(UserPhotoManager userPhotoManager, SettingsManager settingsManager, UserPhotoThumbnailSettings thumbnailSettings, Guid userId)
    {
        var thumbnailsData = new ThumbnailsData(userId, userPhotoManager);

        var resultBitmaps = new List<ThumbnailItem>();

        var (mainImg, _) = await thumbnailsData.MainImgBitmapAsync();

        using var img = mainImg;

        if (img == null)
        {
            return null;
        }

        if (thumbnailSettings.Size.IsEmpty)
        {
            thumbnailSettings.Size = new Size(img.Width, img.Height);
        }

        foreach (var thumbnail in await thumbnailsData.ThumbnailList())
        {
            thumbnail.Image = GetImage(img, thumbnail.Size, thumbnailSettings);

            resultBitmaps.Add(thumbnail);
        }

        await thumbnailsData.SaveAsync(resultBitmaps);

        await settingsManager.SaveAsync(thumbnailSettings, userId);

        return await thumbnailsData.ThumbnailList();
    }

    public static Image GetImage(Image mainImg, Size size, UserPhotoThumbnailSettings thumbnailSettings)
    {
        var x = thumbnailSettings.Point.X > 0 ? thumbnailSettings.Point.X : 0;
        var y = thumbnailSettings.Point.Y > 0 ? thumbnailSettings.Point.Y : 0;
        var width = x + thumbnailSettings.Size.Width > mainImg.Width ? mainImg.Width : thumbnailSettings.Size.Width;
        var height = y + thumbnailSettings.Size.Height > mainImg.Height ? mainImg.Height : thumbnailSettings.Size.Height;

        var rect = new Rectangle(x,
                                 y,
                                 width,
                                 height);

        var result = mainImg.Clone(img => img.BackgroundColor(Color.White).Crop(rect).Resize(new ResizeOptions
        {
            Size = size
        }));

        return result;
    }

    public static void CheckImgFormat(byte[] data)
    {
        IImageFormat imgFormat;
        try
        {
            using var img = Image.Load(data);
            imgFormat = img.Metadata.DecodedImageFormat;
        }
        catch (OutOfMemoryException)
        {
            throw new ImageSizeLimitException();
        }
        catch (ArgumentException error)
        {
            throw new UnknownImageFormatException(error);
        }

        if (imgFormat.Name != "PNG" && imgFormat.Name != "JPEG")
        {
            throw new UnknownImageFormatException();
        }
    }

    public static byte[] TryParseImage(byte[] data, long maxFileSize, Size maxsize)
    {
        if (data == null || data.Length <= 0)
        {
            throw new UnknownImageFormatException();
        }

        if (maxFileSize != -1 && data.Length > maxFileSize)
        {
            throw new ImageSizeLimitException();
        }

        //data = ImageHelper.RotateImageByExifOrientationData(data, Log);

        try
        {
            using var img = Image.Load(data);
            var width = img.Width;
            var height = img.Height;
            var maxWidth = maxsize.Width;
            var maxHeight = maxsize.Height;

            if ((maxHeight != -1 && img.Height > maxHeight) || (maxWidth != -1 && img.Width > maxWidth))
            {
                #region calulate height and width

                if (width > maxWidth && height > maxHeight)
                {

                    if (width > height)
                    {
                        height = (int)(height * (double)maxWidth / width + 0.5);
                        width = maxWidth;
                    }
                    else
                    {
                        width = (int)(width * (double)maxHeight / height + 0.5);
                        height = maxHeight;
                    }
                }

                if (width > maxWidth && height <= maxHeight)
                {
                    height = (int)(height * (double)maxWidth / width + 0.5);
                    width = maxWidth;
                }

                if (width <= maxWidth && height > maxHeight)
                {
                    width = (int)(width * (double)maxHeight / height + 0.5);
                    height = maxHeight;
                }

                var tmpW = width;
                var tmpH = height;
                #endregion
                using var destRound = img.Clone(x => x.Resize(new ResizeOptions
                {
                    Size = new Size(tmpW, tmpH),
                    Mode = ResizeMode.Stretch
                }));

                data = CommonPhotoManager.SaveToBytes(destRound);
            }
            return data;
        }
        catch (OutOfMemoryException)
        {
            throw new ImageSizeLimitException();
        }
        catch (ArgumentException error)
        {
            throw new UnknownImageFormatException(error);
        }
    }
}

public class ThumbnailItem
{
    public Size Size { get; init; }
    public string ImgUrl { get; set; }
    public Image Image { get; set; }
}

public class ThumbnailsData(Guid userId, UserPhotoManager userPhotoManager)
{
    public async Task<(Image, IImageFormat)> MainImgBitmapAsync()
    {
        var (img, imageFormat) = await userPhotoManager.GetPhotoImageAsync(userId);
        return (img, imageFormat);
    }

    public async Task<List<ThumbnailItem>> ThumbnailList()
    {
        return
        [
            new() { Size = UserPhotoManager.RetinaFotoSize, ImgUrl = await userPhotoManager.GetRetinaPhotoURL(userId) },

            new() { Size = UserPhotoManager.MaxFotoSize, ImgUrl = await userPhotoManager.GetMaxPhotoURL(userId) },

            new() { Size = UserPhotoManager.BigFotoSize, ImgUrl = await userPhotoManager.GetBigPhotoURL(userId) },

            new() { Size = UserPhotoManager.MediumFotoSize, ImgUrl = await userPhotoManager.GetMediumPhotoURL(userId) },

            new() { Size = UserPhotoManager.SmallFotoSize, ImgUrl = await userPhotoManager.GetSmallPhotoURL(userId) }
        ];
    }

    public async Task SaveAsync(List<ThumbnailItem> bitmaps)
    {
        foreach (var item in bitmaps)
        {
            var (mainImgBitmap, format) = await MainImgBitmapAsync();
            using var mainImg = mainImgBitmap;
            await userPhotoManager.SaveThumbnail(userId, item.Image, format);
        }
    }
}
