// (c) Copyright Ascensio System SIA 2010-2022
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

global using System.Net;
global using System.Reflection;
global using System.Text.RegularExpressions;

global using ASC.Api.Core;
global using ASC.Api.Core.Extensions;
global using ASC.Common;
global using ASC.Common.Caching;
global using ASC.Common.Log;
global using ASC.Core;
global using ASC.Core.Common.Configuration;
global using ASC.Core.Common.Notify;
global using ASC.Core.Common.Notify.IntegrationEvents.Events;
global using ASC.Core.Common.Notify.Telegram;
global using ASC.EventBus.Abstractions;
global using ASC.EventBus.Log;
global using ASC.FederatedLogin.LoginProviders;
global using ASC.Notify.Messages;
global using ASC.TelegramService;
global using ASC.TelegramService.Core;
global using ASC.TelegramService.IntegrationEvents.EventHandling;
global using ASC.TelegramService.Log;

global using Autofac;

global using Microsoft.AspNetCore.Builder;
global using Microsoft.Extensions.Caching.Distributed;
global using Microsoft.Extensions.Hosting.WindowsServices;
global using Microsoft.Extensions.Logging;

global using Telegram.Bot;
global using Telegram.Bot.Exceptions;
global using Telegram.Bot.Types;
global using Telegram.Bot.Types.Enums;
global using ASC.TelegramService.Extension;
