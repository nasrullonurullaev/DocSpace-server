const path = require("path");
const beforeBuild = require("@docspace/common/utils/beforeBuild");

beforeBuild(
  [
    path.join(__dirname, "../public/locales"),
    path.join(__dirname, "../../../public/locales"),
  ],
  path.join(__dirname, "../src/helpers/autoGeneratedTranslations.js")
);
