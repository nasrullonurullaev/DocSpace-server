import React, { useState, useEffect, useRef } from "react";
import ModalDialog from "@docspace/components/modal-dialog";
import Button from "@docspace/components/button";
import { LabledInput } from "./LabledInput";
import styled from "styled-components";
import { Hint } from "../styled-components";
import { SSLVerification } from "./SSLVerification";
import SecretKeyInput from "./SecretKeyInput";
import { useTranslation } from "react-i18next";

const Footer = styled.div`
  width: 100%;
  display: flex;

  button {
    width: 100%;
  }
  button:first-of-type {
    margin-right: 10px;
  }
`;

function validateUrl(url) {
  try {
    new URL(url);
  } catch (error) {
    return false;
  }
  return true;
}

const WebhookDialog = (props) => {
  const { visible, onClose, header, isSettingsModal, onSubmit, webhook } = props;

  const [isResetVisible, setIsResetVisible] = useState(isSettingsModal);
  const [isUrlValid, setIsUrlValid] = useState(false);
  const [isPasswordValid, setIsPasswordValid] = useState(false);

  const { t } = useTranslation(["Webhooks", "Common"]);

  const [webhookInfo, setWebhookInfo] = useState({
    id: webhook ? webhook.id : 0,
    name: webhook ? webhook.name : "",
    uri: webhook ? webhook.uri : "",
    secretKey: webhook ? webhook.secretKey : "",
    enabled: webhook ? webhook.enabled : true,
    ssl: webhook ? webhook.ssl : true,
  });

  const submitButtonRef = useRef(null);

  const onModalClose = () => {
    onClose();
    isSettingsModal && setIsResetVisible(true);
  };

  const onInputChange = (e) => {
    e.target.name &&
      setWebhookInfo((prevWebhookInfo) => ({
        ...prevWebhookInfo,
        [e.target.name]: e.target.value,
      }));
  };

  const handleSubmitClick = () => {
    if (isUrlValid && (isPasswordValid || isResetVisible)) {
      submitButtonRef.current.click();
    }
  };

  const onFormSubmit = (e) => {
    e.preventDefault();
    onSubmit(webhookInfo);
    setWebhookInfo({
      id: webhook ? webhook.id : 0,
      name: webhook ? webhook.name : "",
      uri: webhook ? webhook.uri : "",
      secretKey: webhook ? webhook.secretKey : "",
      enabled: webhook ? webhook.enabled : true,
    });
    onModalClose();
  };

  useEffect(() => {
    window.addEventListener("keyup", onKeyPress);
    return () => window.removeEventListener("keyup", onKeyPress);
  }, []);

  useEffect(() => {
    setWebhookInfo({
      id: webhook ? webhook.id : 0,
      name: webhook ? webhook.name : "",
      uri: webhook ? webhook.uri : "",
      secretKey: webhook ? webhook.secretKey : "",
      enabled: webhook ? webhook.enabled : true,
      ssl: webhook ? webhook.ssl : true,
    });
  }, [webhook]);

  useEffect(() => {
    const isValid = validateUrl(webhookInfo.uri);
    isValid !== isUrlValid && setIsUrlValid(isValid);
  }, [webhookInfo.uri]);

  const onKeyPress = (e) => (e.key === "Esc" || e.key === "Escape") && onModalClose();

  return (
    <ModalDialog withFooterBorder visible={visible} onClose={onModalClose} displayType="aside">
      <ModalDialog.Header>{header}</ModalDialog.Header>
      <ModalDialog.Body>
        <form onSubmit={onFormSubmit}>
          <Hint>{t("WebhookCreationHint")}</Hint>
          <LabledInput
            label={t("WebhookName")}
            placeholder={t("EnterWebhookName")}
            name="name"
            value={webhookInfo.name}
            onChange={onInputChange}
            required
          />
          <LabledInput
            label={t("PayloadUrl")}
            placeholder={t("EnterUrl")}
            name="uri"
            value={webhookInfo.uri}
            onChange={onInputChange}
            hasError={!isUrlValid}
            required
          />
          <SSLVerification value={webhookInfo.ssl} onChange={onInputChange} />
          <SecretKeyInput
            isResetVisible={isResetVisible}
            name="secretKey"
            value={webhookInfo.secretKey}
            onChange={onInputChange}
            isPasswordValid={isPasswordValid}
            setIsPasswordValid={setIsPasswordValid}
            setIsResetVisible={setIsResetVisible}
          />

          <button type="submit" ref={submitButtonRef} hidden></button>
        </form>
      </ModalDialog.Body>

      <ModalDialog.Footer>
        <Footer>
          <Button
            label={isSettingsModal ? t("Save") : t("Common:Create")}
            size="normal"
            primary={true}
            onClick={handleSubmitClick}
          />
          <Button label={t("Cancel")} size="normal" onClick={onModalClose} />
        </Footer>
      </ModalDialog.Footer>
    </ModalDialog>
  );
};

export default WebhookDialog;
