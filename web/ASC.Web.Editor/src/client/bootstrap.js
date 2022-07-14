import React, { Suspense } from "react";
import { hydrate } from "react-dom";
import { registerSW } from "@appserver/common/sw/helper";
import { App } from "./App.js";
import Editor from "./Editor";
import { useSSR } from "react-i18next";
import useMfScripts from "../helpers/useMfScripts";
import initDesktop from "../helpers/initDesktop";
import { AppServerConfig } from "@appserver/common/constants";
import { combineUrl } from "@appserver/common/utils";
import ErrorBoundary from "../components/ErrorBoundary";
import pkg from "../../package.json";

const propsObj = window.__ASC_INITIAL_STATE__.props;
const initialI18nStore = window.initialI18nStore;
const initialLanguage = window.initialLanguage;
const socketPath = pkg.socketPath;

const isDesktopEditor = window["AscDesktopEditor"] !== undefined;

const AppWrapper = () => {
  const [isInitialized, isErrorLoading] = useMfScripts();
  useSSR(initialI18nStore, initialLanguage);
  React.useEffect(() => {
    const tempElm = document.getElementById("loader");
    if (tempElm) {
      tempElm.outerHTML = "";
    }
  }, []);
  const onError = () =>
    window.open(
      combineUrl(
        AppServerConfig.proxyURL,
        propsObj.personal ? "sign-in" : "/login"
      ),
      "_self"
    );
  console.log(propsObj);
  return (
    <ErrorBoundary onError={onError}>
      <Suspense fallback={<div />}>
        <Editor
          {...propsObj}
          mfReady={isInitialized}
          mfFailed={isErrorLoading}
          isDesktopEditor={isDesktopEditor}
          initDesktop={initDesktop}
        />
      </Suspense>
    </ErrorBoundary>
  );
};

hydrate(<AppWrapper />, document.getElementById("root"));

if (IS_DEVELOPMENT) {
  const port = PORT || 5013;
  const ws = new WebSocket(`ws://localhost:${port}${socketPath}`);
  let isErrorConnection = false;

  ws.onopen = (event) => {
    console.log("[editor-dev] Socket is connected. Live reload enabled");
  };

  ws.onmessage = function (event) {
    if (event.data === "reload") {
      console.log("[editor-dev] App updated. Reloading...");
      location.reload();
    }
  };

  ws.onclose = function (event) {
    console.log("close");
    console.log("[editor-dev] Socket is disconnected! Reloading...");
    setTimeout(() => {
      !isErrorConnection && location.reload();
    }, 1000);
  };

  ws.onerror = (event) => {
    isErrorConnection = true;
    console.log("[editor-dev] Socket connect error!");
  };
}

registerSW();
