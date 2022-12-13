import { makeAutoObservable } from "mobx";

import {
  EmployeeActivationStatus,
  EmployeeStatus,
  FolderType,
  ShareAccessRights,
} from "@docspace/common/constants";
import {
  getFileRoleActions,
  getRoomRoleActions,
  getArchiveRoomRoleActions,
  getArchiveFileRoleActions,
} from "@docspace/common/utils/actions";

class AccessRightsStore {
  authStore = null;
  selectedFolderStore = null;
  treeFoldersStore = null;

  constructor(authStore, selectedFolderStore) {
    this.authStore = authStore;
    this.selectedFolderStore = selectedFolderStore;

    makeAutoObservable(this);
  }

  canInviteUserInRoom(room) {
    const { security } = room;

    return security?.EditAccess;
  }

  canChangeUserRoleInRoom = (room) => {
    const { currentUserInList, security } = room;
    const { userStore } = this.authStore;
    const { user } = userStore;

    const isMyProfile = user.id === currentUserInList.id;
    const isOwnerRoleRoom =
      currentUserInList.access === ShareAccessRights.FullAccess;

    if (isMyProfile || isOwnerRoleRoom) return false;

    return security?.EditAccess;
  };
  canDeleteUserInRoom = (room) => {
    const { currentUserInList, security } = room;
    const { userStore } = this.authStore;
    const { user } = userStore;

    const isMyProfile = user.id === currentUserInList.id;
    const isOwnerRoleRoom =
      currentUserInList.access === ShareAccessRights.FullAccess;

    if (isMyProfile || isOwnerRoleRoom) return false;

    return security?.EditAccess;
  };
  canLockFile = (file) => {
    const { security } = file;

    // if (rootFolderType === FolderType.USER) return false;

    return security?.Lock;
  };

  canChangeVersionFileHistory = (file) => {
    const { editing, security } = file;

    if (editing) return false;

    return security?.EditHistory;
  };

  canViewVersionFileHistory = (file) => {
    const { security } = file;

    return security?.ReadHistory;
  };

  canEditFile = (file) => {
    const { security } = file;

    return security?.Edit;
  };

  canRenameItem = (item = {}) => {
    const { security } = item;

    return security?.Rename;
  };

  canFillForm = (file) => {
    const { rootFolderType, access } = file;

    if (rootFolderType === FolderType.Archive)
      return getArchiveFileRoleActions(access).fillForm;

    if (rootFolderType === FolderType.TRASH) return false;

    return getFileRoleActions(access).fillForm;
  };

  canMakeForm = (item) => {
    const { rootFolderType, access } = item;

    if (rootFolderType === FolderType.Archive)
      return getArchiveFileRoleActions(access).saveAsForm;

    if (
      rootFolderType === FolderType.TRASH ||
      // rootFolderType === FolderType.Privacy ||
      rootFolderType === FolderType.Favorites ||
      rootFolderType === FolderType.Recent
    )
      return false;

    return getFileRoleActions(access).saveAsForm;
  };

  canArchiveRoom = (room) => {
    const { security } = room;

    return security?.Move;
  };

  canRemoveRoom = (room) => {
    const { security } = room;

    return security?.Delete;
  };

  canViewRoomInfo = (room) => {
    const { access, rootFolderType } = room;

    if (rootFolderType === FolderType.Archive)
      return getArchiveRoomRoleActions(access).viewInfo;

    return getRoomRoleActions(access).viewInfo;
  };

  canPinRoom = (room) => {
    const { security } = room;

    return security?.Pin;
  };

  canEditRoom = (room) => {
    const { security } = room;

    return security?.EditRoom;
  };

  get canCreateFiles() {
    const { access, rootFolderType } = this.selectedFolderStore;

    if (rootFolderType === FolderType.Archive)
      return getArchiveFileRoleActions(access).create;

    const { create } = getFileRoleActions(access);

    return create;
  }

  canMoveItems = (item) => {
    const { editing: fileEditing, security } = item;

    if (fileEditing) return false;

    return security?.Move;
  };

  canDeleteItems = (item) => {
    const { editing: fileEditing, security } = item;

    if (fileEditing) return false;

    return security?.Delete;
  };

  canCopyItems = (item) => {
    const { security } = item;

    return security?.Copy;
  };
  canDuplicateFile = (item) => {
    const { rootFolderType, access } = item;

    if (rootFolderType === FolderType.Archive)
      return getArchiveFileRoleActions(access).canDuplicate;

    if (
      rootFolderType === FolderType.TRASH ||
      rootFolderType === FolderType.Favorites ||
      rootFolderType === FolderType.Recent
      // || rootFolderType === FolderType.Privacy
    )
      return false;

    return getFileRoleActions(access).canDuplicate;
  };
  canChangeUserType = (user) => {
    const { id, isOwner } = this.authStore.userStore.user;

    const { id: userId, statusType, role } = user;

    if (userId === id || statusType === EmployeeStatus.Disabled) return false;

    switch (role) {
      case "owner":
        return false;

      case "admin":
      case "manager":
        if (isOwner) {
          return true;
        } else {
          return false;
        }

      case "user":
        return true;

      default:
        return false;
    }
  };

  canMakeEmployeeUser = (user) => {
    const { id, isOwner } = this.authStore.userStore.user;

    const {
      status,
      id: userId,
      isAdmin: userIsAdmin,
      isOwner: userIsOwner,
      isVisitor: userIsVisitor,
    } = user;

    const needMakeEmployee =
      status !== EmployeeStatus.Disabled && userId !== id;

    if (isOwner) return needMakeEmployee;

    return needMakeEmployee && !userIsAdmin && !userIsOwner && userIsVisitor;
  };

  canActivateUser = (user) => {
    const { id, isOwner, isAdmin } = this.authStore.userStore.user;

    const {
      status,
      id: userId,
      isAdmin: userIsAdmin,
      isOwner: userIsOwner,
    } = user;

    const needActivate = status !== EmployeeStatus.Active && userId !== id;

    if (isOwner) return needActivate;

    if (isAdmin) return needActivate && !userIsAdmin && !userIsOwner;

    return false;
  };

  canDisableUser = (user) => {
    const { id, isOwner, isAdmin } = this.authStore.userStore.user;

    const {
      status,
      id: userId,
      isAdmin: userIsAdmin,
      isOwner: userIsOwner,
    } = user;

    const needDisable = status !== EmployeeStatus.Disabled && userId !== id;

    if (isOwner) return needDisable;

    if (isAdmin) return needDisable && !userIsAdmin && !userIsOwner;

    return false;
  };

  canInviteUser = (user) => {
    const { id, isOwner } = this.authStore.userStore.user;

    const {
      activationStatus,
      status,
      id: userId,
      isAdmin: userIsAdmin,
      isOwner: userIsOwner,
    } = user;

    const needInvite =
      activationStatus === EmployeeActivationStatus.Pending &&
      status === EmployeeStatus.Active &&
      userId !== id;

    if (isOwner) return needInvite;

    return needInvite && !userIsAdmin && !userIsOwner;
  };

  canRemoveUser = (user) => {
    const { id, isOwner, isAdmin } = this.authStore.userStore.user;

    const {
      status,
      id: userId,
      isAdmin: userIsAdmin,
      isOwner: userIsOwner,
    } = user;

    const needRemove = status === EmployeeStatus.Disabled && userId !== id;

    if (isOwner) return needRemove;

    if (isAdmin) return needRemove && !userIsAdmin && !userIsOwner;

    return false;
  };

  canViewUsers = (room) => {
    const { rootFolderType } = this.selectedFolderStore;

    if (!room) return false;

    const options =
      rootFolderType === FolderType.Archive
        ? getArchiveRoomRoleActions(room.access)
        : getRoomRoleActions(room.access);

    return options.viewUsers;
  };
}

export default AccessRightsStore;
