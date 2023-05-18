import React from "react";
import styled, { css } from "styled-components";
import { Text, Textarea, Button } from "@docspace/components";

import json_beautifier from "csvjson-json_beautifier";
import { isMobileOnly } from "react-device-detect";
import { useTranslation } from "react-i18next";

const DetailsWrapper = styled.div`
  width: 100%;

  .textareaBody {
    height: 50vh !important;
  }
`;

const LargePayloadStub = styled.div`
  box-sizing: border-box;

  display: flex;
  justify-content: space-between;
  align-items: center;

  width: 100%;
  max-width: 1200px;
  padding: 12px 10px;
  margin-top: 4px;

  background: #f8f9f9;
  border: 1px solid #eceef1;
  border-radius: 3px;

  ${isMobileOnly &&
  css`
    justify-content: flex-start;
    flex-wrap: wrap;
    row-gap: 16px;
  `}
`;

function isJSON(jsonString) {
  try {
    const parsedJson = JSON.parse(jsonString);
    return parsedJson && typeof parsedJson === "object";
  } catch (e) {}

  return false;
}

export const ResponseDetails = ({ webhookDetails }) => {
  const responsePayload = webhookDetails.responsePayload?.trim();
  const { t } = useTranslation(["Webhooks"]);

  const beautifiedJSON = isJSON(responsePayload)
    ? json_beautifier(JSON.parse(responsePayload), {
        inlineShortArrays: true,
      })
    : responsePayload;

  const numberOfLines = isJSON(responsePayload)
    ? beautifiedJSON.split("\n").length
    : responsePayload.split("\n").length;

  const openRawPayload = () => {
    const rawPayload = window.open("");
    isJSON(responsePayload)
      ? rawPayload.document.write(beautifiedJSON.replace(/(?:\r\n|\r|\n)/g, "<br/>"))
      : rawPayload.document.write(responsePayload);
    rawPayload.focus();
  };

  return (
    <DetailsWrapper>
      <Text as="h3" fontWeight={600} style={{ marginBottom: "4px" }}>
        {t("ResponsePostHeader")}
      </Text>
      <Textarea
        value={webhookDetails.responseHeaders}
        enableCopy
        hasNumeration
        isFullHeight
        isJSONField
        copyInfoText={t("ResponseHeaderCopied")}
      />
      <Text as="h3" fontWeight={600} style={{ marginBottom: "4px", marginTop: "16px" }}>
        {t("ResponsePostBody")}
      </Text>
      {responsePayload.length > 4000 || numberOfLines > 100 ? (
        <LargePayloadStub>
          <Text fontWeight={600} color="#657077">
            {t("PayloadIsTooLarge")}
          </Text>
          <Button
            size="small"
            onClick={openRawPayload}
            label={t("ViewRawPayload")}
            scale={isMobileOnly}
          />
        </LargePayloadStub>
      ) : responsePayload === "" ? (
        <Textarea isDisabled />
      ) : isJSON(responsePayload) ? (
        <Textarea
          value={responsePayload}
          isJSONField
          enableCopy
          hasNumeration
          isFullHeight
          copyInfoText={t("ResponseBodyCopied")}
        />
      ) : (
        <Textarea
          value={responsePayload}
          enableCopy
          heightScale
          className="textareaBody"
          copyInfoText={t("ResponseBodyCopied")}
        />
      )}
    </DetailsWrapper>
  );
};
