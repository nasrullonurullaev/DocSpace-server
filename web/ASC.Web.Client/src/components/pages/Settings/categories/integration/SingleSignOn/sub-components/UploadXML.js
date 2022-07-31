import React from "react";
import styled, { css } from "styled-components";
import { inject, observer } from "mobx-react";
import { useTranslation } from "react-i18next";

import Box from "@appserver/components/box";
import Button from "@appserver/components/button";
import FieldContainer from "@appserver/components/field-container";
import Text from "@appserver/components/text";

import SsoTextInput from "./SsoTextInput";
import FileInput from "@appserver/components/file-input";
import UploadIcon from "../../../../../../../../public/images/actions.upload.react.svg";
import { Base } from "@appserver/components/themes";

const StyledUploadIcon = styled(UploadIcon)`
  path {
    stroke: ${(props) =>
      props.isDisabled
        ? props.theme.studio.settings.integration.sso.iconButtonDisabled
        : props.theme.studio.settings.integration.sso.iconButton} !important;
  }
`;

StyledUploadIcon.defaultProps = { theme: Base };

const UploadXML = (props) => {
  const { t } = useTranslation(["SingleSignOn", "Common"]);
  const {
    enableSso,
    uploadXmlUrl,
    isLoadingXml,
    uploadByUrl,
    uploadXml,
  } = props;

  return (
    <FieldContainer
      className="xml-input"
      errorMessage="Error text. Lorem ipsum dolor sit amet, consectetuer adipiscing elit"
      isVertical
      labelText={t("UploadXML")}
    >
      <Box alignItems="center" displayProp="flex" flexDirection="row">
        <SsoTextInput
          maxWidth="300px"
          name="uploadXmlUrl"
          placeholder={t("UploadXMLPlaceholder")}
          tabIndex={1}
          value={uploadXmlUrl}
        />

        <Button
          className="upload-button"
          icon={
            <StyledUploadIcon
              isDisabled={
                !enableSso || uploadXmlUrl.trim().length === 0 || isLoadingXml
              }
            />
          }
          isDisabled={
            !enableSso || uploadXmlUrl.trim().length === 0 || isLoadingXml
          }
          onClick={uploadByUrl}
          size="small"
          tabIndex={2}
        />

        <Text className="or-text" noSelect>
          {t("Or")}
        </Text>

        <FileInput
          accept=".xml"
          buttonLabel={t("Common:SelectFile")}
          className="xml-upload-file"
          isDisabled={!enableSso || isLoadingXml}
          onInput={uploadXml}
          size="middle"
          tabIndex={3}
        />
      </Box>
    </FieldContainer>
  );
};

export default inject(({ ssoStore }) => {
  const {
    enableSso,
    uploadXmlUrl,
    isLoadingXml,
    uploadByUrl,
    uploadXml,
  } = ssoStore;

  return {
    enableSso,
    uploadXmlUrl,
    isLoadingXml,
    uploadByUrl,
    uploadXml,
  };
})(observer(UploadXML));
