import i18n from "i18next";
import { initReactI18next } from "react-i18next";
import Backend from "@appserver/common/utils/i18next-http-backend";
import { LANGUAGE } from "@appserver/common/constants";
import config from "../../../../package.json";
import { loadLanguagePath } from "@appserver/common/utils";

const newInstance = i18n.createInstance();

newInstance
  .use(Backend)
  .use(initReactI18next)
  .init({
    lng: localStorage.getItem(LANGUAGE) || "en",
    fallbackLng: "en",
    load: "currentOnly",
    //debug: true,

    interpolation: {
      escapeValue: false, // not needed for react as it escapes by default
      format: function (value, format) {
        if (format === "lowercase") return value.toLowerCase();
        return value;
      },
    },

    backend: {
      loadPath: loadLanguagePath(config.homepage, "About"),
    },

    react: {
      useSuspense: false,
    },
  });

export default newInstance;
