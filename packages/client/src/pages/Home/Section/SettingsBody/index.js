import React, { useCallback } from "react";
import { useTranslation } from "react-i18next";
import styled, { css } from "styled-components";
import { useNavigate } from "react-router-dom";
import Error520 from "client/Error520";
import { inject, observer } from "mobx-react";
import { combineUrl } from "@docspace/common/utils";
import config from "PACKAGE_FILE";
import Submenu from "@docspace/components/submenu";
import PersonalSettings from "./CommonSettings";
import GeneralSettings from "./AdminSettings";
import { tablet } from "@docspace/components/utils/device";
import { isMobile } from "react-device-detect";
import PluginsSettings from "./PluginsSettings";

const StyledContainer = styled.div`
  margin-top: -22px;

  @media ${tablet} {
    margin-top: 0px;
  }

  ${isMobile &&
  css`
    margin-top: 0px;
  `}
`;

const SectionBodyContent = ({
  isErrorSettings,
  user,
  enablePlugins,
  plugins,
}) => {
  const { t } = useTranslation(["FilesSettings", "Common", "PluginsSettings"]);

  const navigate = useNavigate();

  const setting = window.location.pathname.endsWith("/settings/personal")
    ? "personal"
    : window.location.pathname.endsWith("/settings/plugins")
    ? "plugins"
    : "general";

  const commonSettings = {
    id: "personal",
    name: t("Common:SettingsPersonal"),
    content: <PersonalSettings t={t} />,
  };

  const adminSettings = {
    id: "general",
    name: t("Common:SettingsGeneral"),
    content: <GeneralSettings t={t} />,
  };

  const pluginsSettings = {
    id: "plugins",
    name: t("PluginsSettings:Plugins"),
    content: <PluginsSettings />,
  };

  const data = [commonSettings];

  if (enablePlugins && (plugins.length > 0 || user.isAdmin || user.isOwner)) {
    data.push(pluginsSettings);
  }

  const showAdminSettings = user.isAdmin || user.isOwner;

  if (showAdminSettings) {
    data.unshift(adminSettings);
  }

  const onSelect = useCallback(
    (e) => {
      const { id } = e;

      if (id === setting) return;

      navigate(
        combineUrl(
          window.DocSpaceConfig?.proxy?.url,
          config.homepage,
          `/settings/${id}`
        )
      );
    },
    [setting, navigate]
  );

  return isErrorSettings ? (
    <Error520 />
  ) : (
    <StyledContainer>
      {!showAdminSettings && !enablePlugins ? (
        <PersonalSettings
          t={t}
          showTitle={true}
          showAdminSettings={showAdminSettings}
        />
      ) : (
        <Submenu
          data={data}
          startSelect={
            setting === "personal"
              ? commonSettings
              : setting === "plugins"
              ? pluginsSettings
              : adminSettings
          }
          onSelect={onSelect}
        />
      )}
    </StyledContainer>
  );
};

export default inject(({ auth, settingsStore, pluginStore }) => {
  const { settingsIsLoaded } = settingsStore;
  const { plugins } = pluginStore;

  const { enablePlugins } = auth.settingsStore;

  return {
    settingsIsLoaded,
    user: auth.userStore.user,
    enablePlugins,
    plugins,
  };
})(observer(SectionBodyContent));
