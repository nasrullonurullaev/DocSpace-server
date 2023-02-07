const fs = require("fs");
const path = require("path");

const pathToResultFile = path.join(
  __dirname,
  "..",
  "result",
  "EqualMD5EqualName"
);

const findImagesWithEqualMD5AndEqualName = (images) => {
  const uniqueImg = new Map();

  images.forEach((i) => {
    const oldImg = uniqueImg.get(i.fileName);

    if (oldImg) {
      let skip = false;

      oldImg.forEach(
        (oi) =>
          (skip = skip || oi.md5Hash !== i.md5Hash || oi.fileName != i.fileName)
      );

      if (!skip) {
        const newImg = [...oldImg, i];

        uniqueImg.set(i.fileName, newImg);
      }
    } else {
      uniqueImg.set(i.fileName, [i]);
    }
  });

  fs.writeFileSync(pathToResultFile, "");

  uniqueImg.forEach((value, key) => {
    if (value.length > 1) {
      let content = `${key}:\n`;

      value.forEach((v) => (content += `${v.path} - ${v.md5Hash}\n`));

      fs.appendFileSync(pathToResultFile, content + `\n`);
    }
  });
};

module.exports = { findImagesWithEqualMD5AndEqualName };
