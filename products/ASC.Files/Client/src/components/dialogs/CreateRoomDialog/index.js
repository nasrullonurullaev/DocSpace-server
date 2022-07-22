import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";

import ModalDialog from "@appserver/components/modal-dialog";

import SetRoomParams from "./views/CreateRoom/SetRoomParams";
import RoomTypeList from "./views/ChooseRoomType/RoomTypeList";
import Button from "@appserver/components/button";
import { roomTypes } from "./data";
import TagHandler from "./handlers/tagHandler";
import { isMobile } from "@appserver/components/utils/device";

const StyledModalDialog = styled(ModalDialog)`
  .modal-scroll {
    .scroll-body {
    }
  }
`;
const CreateRoomDialog = ({
  t,
  visible,
  setCreateRoomDialogVisible,
  createRoom,
}) => {
  const onClose = () => setCreateRoomDialogVisible(false);
  const [isScrollEnabled, setIsScrollEnabled] = useState(false);

  const [roomParams, setRoomParams] = useState({
    title: "",
    type: undefined,
    tags: [],
    isPrivate: false,
    storageLocation: undefined,
    icon: "",
  });

  const setRoomTags = (newTags) =>
    setRoomParams({ ...roomParams, tags: newTags });

  const tagHandler = new TagHandler(roomParams.tags, setRoomTags);

  const setRoomType = (newRoomType) => {
    const [roomByType] = roomTypes.filter((room) => room.type === newRoomType);
    tagHandler.refreshDefaultTag(t(roomByType.title));
    setRoomParams((prev) => ({
      ...prev,
      type: newRoomType,
    }));
  };

  const onCreateRoom = () => {
    console.log(roomParams);
  };

  const isChooseRoomType = roomParams.type === undefined;
  return (
    <StyledModalDialog
      displayType="aside"
      withBodyScroll
      visible={visible}
      onClose={onClose}
      isScrollLocked={roomParams.isPrivate}
    >
      <ModalDialog.Header>
        {isChooseRoomType ? t("ChooseRoomType") : t("CreateRoom")}
      </ModalDialog.Header>

      <ModalDialog.Body>
        {isChooseRoomType ? (
          <RoomTypeList t={t} setRoomType={setRoomType} />
        ) : (
          <SetRoomParams
            t={t}
            tagHandler={tagHandler}
            roomParams={roomParams}
            setRoomParams={setRoomParams}
            setRoomType={setRoomType}
          />
        )}
      </ModalDialog.Body>

      {!isChooseRoomType && (
        <ModalDialog.Footer>
          <Button
            tabIndex={5}
            label={t("Common:Create")}
            size="normal"
            primary
            scale
            onClick={onCreateRoom}
          />
          <Button
            tabIndex={5}
            label={t("Common:CancelButton")}
            size="normal"
            scale
            onClick={() => unlockScroll()}
          />
        </ModalDialog.Footer>
      )}
    </StyledModalDialog>
  );
};

export default inject(({ dialogsStore, roomsStore }) => {
  const {
    createRoomDialogVisible: visible,
    setCreateRoomDialogVisible,
  } = dialogsStore;

  //const { createRoom } = roomsStore;

  return {
    visible,
    setCreateRoomDialogVisible,
    createRoom: () => {},
  };
})(withTranslation(["CreateRoomDialog", "Common"])(observer(CreateRoomDialog)));
