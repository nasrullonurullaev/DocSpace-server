import React, { useState, useEffect, useCallback } from "react";
import ModalDialog from "@docspace/components/modal-dialog";
import Button from "@docspace/components/button";
import FillingRoleSelector from "@docspace/components/filling-role-selector";
import InviteUserForRolePanel from "../InviteUserForRolePanel";
import Aside from "@docspace/components/aside";
import StartFillingPanelLoader from "@docspace/common/components/Loaders/StartFillingPanelLoader";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import styled from "styled-components";

const StyledModalDialog = styled(ModalDialog)`
  .modal-body {
    padding-left: 0;
    padding-right: 0;
  }

  .scroll-body {
    padding-right: 0 !important;
  }

  .row {
    padding: 0 16px;
  }

  .list-header {
    padding: 8px 16px;
  }

  .tooltip {
    margin: 8px 16px;
  }
`;

const everyoneRole = 1;
const StartFillingPanel = ({
  startFillingPanelVisible,
  setStartFillingPanelVisible,
  isVisible,
  getRolesUsersForFillingForm,
  setRolesUsersForFillingForm,
  fileId = 4,
  roomId = 7,
  getRoomMembers,
  tReady,
  t,
}) => {
  const [visibleInviteUserForRolePanel, setVisibleInviteUserForRolePanel] =
    useState(false);
  const [visibleTooltip, setVisibleTooltip] = useState(true);
  const [addUserToRoomVisible, setAddUserToRoomVisible] = useState(false);

  const [members, setMembers] = useState([]);
  const [currentRole, setCurrentRole] = useState("");
  const [roles, setRoles] = useState([]);
  const [users, setUsers] = useState([]);

  const [isDisabledStart, setIsDisabledStart] = useState(true);
  const [isShowLoader, setIsShowLoader] = useState(true);

  useEffect(() => {
    getRolesUsersForFillingForm(fileId)
      .then((roles) => {
        setRoles(roles);
      })
      .catch((e) => console.log(e));
  }, []);

  useEffect(() => {
    setIsShowLoader(!tReady || !roles.length);
  }, [tReady, roles.length]);

  useEffect(() => {
    Boolean(isVisible) && setStartFillingPanelVisible(isVisible);
  }, [isVisible]);

  useEffect(() => {
    const allRolesFilled = roles.length - everyoneRole === users.length;

    if (allRolesFilled) {
      setIsDisabledStart(false);
    } else {
      setIsDisabledStart(true);
    }
  }, [roles.length, users.length]);

  const fetchMembers = async () => {
    let data = await getRoomMembers(roomId);

    data = data.filter((m) => m.sharedTo.email || m.sharedTo.displayName);
    let inRoomMembers = [];
    data.map((fetchedMember) => {
      const member = {
        label: fetchedMember.sharedTo.displayName,
        ...fetchedMember.sharedTo,
      };
      if (member.activationStatus !== 2) inRoomMembers.push(member);
    });
    setMembers(inRoomMembers);
  };

  const onAddUser = (role) => {
    setCurrentRole(role);
    onOpenInviteUserForRolePanel();
  };

  const onClose = () => {
    setStartFillingPanelVisible(false);
    onCloseInviteUserForRolePanel();
    setCurrentRole("");
  };

  const onOpenInviteUserForRolePanel = () => {
    fetchMembers();
    setVisibleInviteUserForRolePanel(true);
  };

  const onCloseInviteUserForRolePanel = () => {
    setVisibleInviteUserForRolePanel(false);
  };

  const onSelectUserForRole = (user) => {
    setUsers([
      ...users,
      {
        ...user,
        displayName: user.label,
        role: currentRole.title,
        roleId: currentRole.id,
      },
    ]);

    onCloseInviteUserForRolePanel();
    setCurrentRole("");
  };

  const onRemoveUser = useCallback(
    (id) => {
      const filteredUsers = users.filter((user) => user.id !== id);
      setUsers(filteredUsers);
    },
    [users, setUsers]
  );

  const onCloseTooltip = () => {
    setVisibleTooltip(false);
  };

  const onOpenAddUserToRoom = () => {
    setAddUserToRoomVisible(true);
  };

  const onCloseAddUserToRoom = () => {
    setAddUserToRoomVisible(false);
  };

  const onStart = () => {
    const idMembers = members.map((member) => member.id);
    const idUsersRoles = [];

    idUsersRoles.push({ id: everyoneRole, userId: idMembers });
    users.map((user) => {
      idUsersRoles.push({ id: user.roleId, userId: [user.id] });
    });

    setRolesUsersForFillingForm(4, idUsersRoles)
      .then(() => {
        //TODO: Add toast
      })
      .catch((e) => {
        console.log("e", e);
      });
  };

  if (visibleInviteUserForRolePanel) {
    return (
      <InviteUserForRolePanel
        visible={visibleInviteUserForRolePanel}
        members={members}
        currentRole={currentRole}
        onClose={onClose}
        onSelectUserForRole={onSelectUserForRole}
        setVisibleInviteUserForRolePanel={setVisibleInviteUserForRolePanel}
        onCloseInviteUserForRolePanel={onCloseInviteUserForRolePanel}
        addUserToRoomVisible={addUserToRoomVisible}
        onOpenAddUserToRoom={onOpenAddUserToRoom}
        onCloseAddUserToRoom={onCloseAddUserToRoom}
        fetchMembers={fetchMembers}
      />
    );
  }

  return (
    <>
      <Aside visible={true} zIndex={310}>
        {isShowLoader ? (
          <StartFillingPanelLoader
            onClose={onClose}
            isCloseable={!visibleInviteUserForRolePanel}
            visible={true}
          />
        ) : (
          <StyledModalDialog
            displayType="aside"
            visible={true}
            withFooterBorder
            onClose={onClose}
            isCloseable={!visibleInviteUserForRolePanel}
          >
            <ModalDialog.Header>
              {t("StartFillingPanel:StartFilling")}
            </ModalDialog.Header>
            <ModalDialog.Body>
              <FillingRoleSelector
                roles={roles}
                users={users}
                descriptionEveryone={t("StartFillingPanel:DescriptionEveryone")}
                descriptionTooltip={t("StartFillingPanel:DescriptionTooltip")}
                titleTooltip={t("StartFillingPanel:TitleTooltip")}
                listHeader={t("StartFillingPanel:ListHeader")}
                visibleTooltip={visibleTooltip}
                onAddUser={onAddUser}
                onRemoveUser={onRemoveUser}
                onCloseTooltip={onCloseTooltip}
              />
            </ModalDialog.Body>
            <ModalDialog.Footer>
              <Button
                label={t("Common:Start")}
                size="normal"
                isDisabled={isDisabledStart}
                onClick={onStart}
                primary
                scale
              />
              <Button
                label={t("Common:CancelButton")}
                onClick={onClose}
                size="normal"
                scale
              />
            </ModalDialog.Footer>
          </StyledModalDialog>
        )}
      </Aside>
    </>
  );
};

export default inject(({ dialogsStore, filesStore }) => {
  const { startFillingPanelVisible, setStartFillingPanelVisible } =
    dialogsStore;
  const {
    getRolesUsersForFillingForm,
    setRolesUsersForFillingForm,
    getRoomMembers,
  } = filesStore;

  return {
    startFillingPanelVisible,
    setStartFillingPanelVisible,
    getRolesUsersForFillingForm,
    setRolesUsersForFillingForm,
    getRoomMembers,
  };
})(
  withTranslation(["Common", "StartFillingPanel"])(observer(StartFillingPanel))
);
