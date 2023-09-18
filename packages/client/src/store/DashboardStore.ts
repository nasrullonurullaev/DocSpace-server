import axios from "axios";
import { makeAutoObservable, runInAction } from "mobx";
import { NavigateOptions } from "react-router-dom";

import SelectedFolderStore from "./SelectedFolderStore";
import ClientLoadingStore from "./ClientLoadingStore";

import { FolderType } from "@docspace/common/constants";

import api from "@docspace/common/api";

import type { IDashboard, IFileByRole, IRole } from "@docspace/common/Models";
import type {
  RoleQueue,
  Folder as FolderInfoType,
  RoleDefaultType,
  RoleDoneType,
  RoleInterruptedType,
  FileByRoleType,
  ActiveFileType,
} from "@docspace/common/types";
import { RoleTypeEnum } from "@docspace/common/enums";
import { getCategoryUrl } from "SRC_DIR/helpers/utils";
import { CategoryType } from "SRC_DIR/helpers/constants";
import FileByRoleStore from "./FileByRoleStore";

const DASHBOARD_VIEW_AS_KEY = "board-view-as";
const DEFAULT_VIEW_AS_VALUE = "dashboard";

class DashboardStore {
  private _roles: RoleQueue[] = [];

  public viewAs!: string;
  public dashboard?: IDashboard;
  public SelectedRolesMap: Map<number, IRole> = new Map();
  public BufferSelectionRole?: IRole;

  public filesByRole = new Map<number, FileByRoleType[]>();
  public selectedFilesByRoleMap: Map<number, IFileByRole> = new Map();
  public collectionFileByRoleStore: Map<number, FileByRoleStore> = new Map();
  public BufferSelectionFilesByRole?: IFileByRole;
  public activeFilesByRole: Map<number, ActiveFileType> = new Map();

  constructor(
    private selectedFolderStore: SelectedFolderStore,
    private clientLoadingStore: ClientLoadingStore
  ) {
    makeAutoObservable(this);
    this.initViewAs();
  }

  //#region private method

  private initViewAs = (): void => {
    const viewAs =
      localStorage.getItem(DASHBOARD_VIEW_AS_KEY) ?? DEFAULT_VIEW_AS_VALUE;

    this.viewAs = viewAs;
  };

  private createCollectionFileByRoleStore = (roles: RoleQueue[]) => {
    this.collectionFileByRoleStore = new Map(
      roles.map((role) => [role.id, new FileByRoleStore(this, role)])
    );
  };
  private refreshCollectionFileByRoleStore = (roles: RoleQueue[]) => {
    this.collectionFileByRoleStore = new Map(
      roles.map((role) => {
        const store = this.collectionFileByRoleStore.get(role.id);

        return [role.id, store ? store : new FileByRoleStore(this, role)];
      })
    );
  };

  private settingUpNavigationPath = async (dashboard: IDashboard) => {
    const navigationPath = await Promise.all(
      dashboard.pathParts.map(async (folder) => {
        const { Rooms, Archive } = FolderType;

        let folderId = folder;

        const folderInfo: FolderInfoType =
          dashboard.current.id === folderId
            ? dashboard.current
            : await api.files.getFolderInfo(folderId);

        const { id, title, rootFolderId, rootFolderType, roomType } =
          folderInfo;

        const isRootRoom =
          rootFolderId === id &&
          (rootFolderType === Rooms || rootFolderType === Archive);

        return {
          id: folderId,
          title,
          isRoom: !!roomType,
          isRootRoom,
        };
      })
    ).then((res) => {
      return res.slice(0, -1).reverse();
    });

    this.selectedFolderStore.setSelectedFolder({
      folders: dashboard.folders,
      ...dashboard.current,
      pathParts: dashboard.pathParts,
      navigationPath: navigationPath,
      isDashboard: true,
      ...{ new: dashboard.new },
    });

    this.clientLoadingStore.setIsSectionHeaderLoading(false);
  };

  private removeOptions = (options: string[], toRemoveArray: string[]) =>
    options.filter((o) => !toRemoveArray.includes(o));

  private goTo = (url: string, options?: NavigateOptions) => {
    window?.DocSpace?.navigate(url, options);
  };

  private setBufferSelection = (
    role: IRole,
    checked: boolean,
    withSelection?: boolean
  ) => {
    const hasRole = this.SelectedRolesMap.has(role.id);

    if (withSelection && hasRole) {
      this.clearBufferSelectionRole();
    }

    if (withSelection && !hasRole) {
      this.BufferSelectionRole = role;
      this.SelectedRolesMap.clear();
    }

    if (!withSelection) {
      this.BufferSelectionRole = role;
      this.SelectedRolesMap.clear();
      if (checked) this.SelectedRolesMap.set(role.id, role);
    }
  };

  //#endregion

  //#region getter

  public get roles(): IRole[] {
    if (!this.dashboard) return [];

    const roles = this._roles.map<IRole>(this.convertToRole);

    return roles;
  }

  public get hasBufferSelectionFileByRole() {
    return !!this.BufferSelectionFilesByRole;
  }

  public get hasSelectionFileByRole() {
    return this.selectedFilesByRoleMap.size > 0;
  }

  public get selectedFileLength() {
    return this.selectedFilesByRoleMap.size;
  }

  public get selectedFileTitle() {
    if (
      this.selectedFilesByRoleMap.size === 0 &&
      this.BufferSelectionFilesByRole
    ) {
      return this.BufferSelectionFilesByRole.title;
    }

    return (
      Array.from(this.selectedFilesByRoleMap.values()).find((el) => el.title)
        ?.title || null
    );
  }

  //#endregion

  //#region public method

  public convertToRole = (role: RoleQueue): IRole => {
    const url = getCategoryUrl(
      CategoryType.Role,
      this.dashboard?.current.id,
      role.id
    );

    const general = {
      contextOptionsModel: this.getRolesContextOptionsModel(role),
      onClickBadge: () => {},
      onChecked: this.selectedRole,
      onContentRowCLick: this.setBufferSelection,
      isChecked: this.SelectedRolesMap.has(role.id),
      isActive: this.BufferSelectionRole?.id === role.id,
      url,
    };

    if (role.type === RoleTypeEnum.Default) {
      const defaultRole: RoleDefaultType = {
        ...role,
        ...general,
        onClickLocation: () =>
          this.goTo(url, {
            state: {
              title: role.title,
              isRoot: false,
              isRoom: false,
              fromDashboard: true,
            },
            replace: true,
          }),
      };

      return defaultRole;
    }

    const doneOrInterruptedRole: RoleDoneType | RoleInterruptedType = {
      ...role,
      ...general,
    };

    return doneOrInterruptedRole;
  };

  public getRolesContextOptionsModel = (role: RoleQueue) => {
    let roleOptions = [
      "link-for-room-members",
      "separator0",
      "download-role",
      "copy-role",
      "separator1",
      "delete-role",
    ];

    if (role.type === RoleTypeEnum.Default) {
      roleOptions = this.removeOptions(roleOptions, [
        "separator1",
        "delete-role",
      ]);
    }

    return roleOptions;
  };
  public getFilesContextOptionsMode = (
    file: FileByRoleType,
    role: RoleQueue
  ) => {
    if (role.type === RoleTypeEnum.Default) {
      let options: string[] = [
        "fill-form",
        "preview",
        "separator0",
        "link-for-room-members",
        "separator1",
        "cancel-filling",
      ];

      return options;
    } else {
      let options: string[] = [
        "preview",
        "separator0",
        "link-for-room-members",
        "download-file",
        "move-to",
        "copy-file",
        "separator1",
        "delete",
      ];

      return options;
    }
  };

  public selectedRole = (role: IRole, checked: boolean): void => {
    if (this.BufferSelectionRole) this.clearBufferSelectionRole();

    if (checked) this.SelectedRolesMap.set(role.id, role);
    else this.SelectedRolesMap.delete(role.id);
  };

  public selectedFileByRole = (file: IFileByRole, checked: boolean) => {
    if (this.BufferSelectionFilesByRole) this.clearBufferSelectionFilesByRole();

    if (checked) {
      this.selectedFilesByRoleMap.set(file.id, file);
    } else {
      this.selectedFilesByRoleMap.delete(file.id);
    }
  };

  public setActiveFiles = (activeFilesId: ActiveFileType[]) => {
    if (activeFilesId && Array.isArray(activeFilesId)) {
      this.activeFilesByRole = new Map(
        activeFilesId.map((file) => [file.id, file])
      );
    }
  };

  public addActiveFiles = (
    activeFilesId: number[],
    destFolderId: string | number
  ) => {
    for (const id of activeFilesId) {
      this.activeFilesByRole.set(id, { id, destFolderId });
    }
  };

  public clearActiveFiles = (activeFilesId: number[]) => {
    for (const id of activeFilesId) {
      this.activeFilesByRole.delete(id);
    }
  };

  public removeSelectedFilesByRole = (removeFiles: IFileByRole[]) => {
    for (const file of removeFiles) {
      this.selectedFilesByRoleMap.delete(file.id);
    }
  };

  public removeFilesById = (fileIds: number[]) => {
    const tempMap = new Map(this.filesByRole);

    for (const [key, files] of tempMap) {
      const filteredFiles = files.filter((file) => !fileIds.includes(file.id));

      this.filesByRole.set(key, filteredFiles);
    }

    for (const id of fileIds) {
      this.selectedFilesByRoleMap.delete(id);
    }
    this.clearBufferSelectionFilesByRole();
  };

  public removeFiles = (removeFiles: IFileByRole[]) => {
    if (removeFiles.length === 0) return;

    this.removeSelectedFilesByRole(removeFiles);
    this.clearBufferSelectionFilesByRole();

    const tempMap = removeFiles.reduce<Record<number, IFileByRole[]>>(
      (acc, file) => {
        if (acc[file.roleId]) {
          acc[file.roleId].push(file);
        } else {
          acc[file.roleId] = [file];
        }
        return { ...acc };
      },
      {}
    );

    for (const [key, array] of Object.entries(tempMap)) {
      const files = this.filesByRole.get(+key);

      if (!files) continue;

      const filteredFiles = files.filter(
        (file) => !array.some((item) => item.id === file.id)
      );

      this.filesByRole.set(+key, filteredFiles);
    }
  };

  public resetState = (): void => {
    this.clearSelectedRoleMap();
    this.clearBufferSelectionRole();

    this.filesByRole.clear();
    this.clearSelectedFileByRoleMap();
    this.collectionFileByRoleStore.clear();
    this.clearBufferSelectionFilesByRole();
  };

  public clearSelectedRoleMap = (): void => {
    this.SelectedRolesMap.clear();
  };
  public clearSelectedFileByRoleMap = (): void => {
    this.selectedFilesByRoleMap.clear();
  };

  public clearBufferSelectionRole = (): void => {
    this.BufferSelectionRole = undefined;
  };

  public clearBufferSelectionFilesByRole = (): void => {
    this.BufferSelectionFilesByRole = undefined;
  };

  public setViewAs = (viewAs: string): void => {
    const isNotEmptySelected = this.SelectedRolesMap.size !== 0;

    if (viewAs === "dashboard") {
      isNotEmptySelected && this.clearSelectedRoleMap();
      this.clearBufferSelectionRole();
    }

    this.viewAs = viewAs;
    localStorage.setItem(DASHBOARD_VIEW_AS_KEY, viewAs);
  };

  public setRoles = (roles: RoleQueue[]): void => {
    this._roles = roles;
  };

  public setDashboard = (dashboard: IDashboard): void => {
    this.dashboard = dashboard;
  };

  public setSelectedRolesMap = (selectedRolesMap: Map<number, IRole>) => {
    this.SelectedRolesMap = selectedRolesMap;
  };
  public setSelectedFilesByRoleMap = (
    selectedFilesByRoleMap: Map<number, IFileByRole>
  ): void => {
    this.selectedFilesByRoleMap = selectedFilesByRoleMap;
  };

  public setBufferSelectionFileByRole = (
    file: IFileByRole,
    checked: boolean,
    withSelection?: boolean
  ) => {
    const hasFile = this.selectedFilesByRoleMap.has(file.id);

    if (withSelection && hasFile) {
      this.clearBufferSelectionFilesByRole();
    }

    if (withSelection && !hasFile) {
      this.BufferSelectionFilesByRole = file;
      this.selectedFilesByRoleMap.clear();
    }

    if (!withSelection) {
      this.BufferSelectionFilesByRole = file;
      this.selectedFilesByRoleMap.clear();
      if (checked) this.selectedFilesByRoleMap.set(file.id, file);
    }
  };

  public fetchFilesByRole = async (
    role: IRole | RoleQueue,
    signal?: AbortSignal
  ): Promise<FileByRoleType[]> => {
    const boardId = this.dashboard?.current.id;

    if (!boardId) return Promise.reject();

    try {
      const files: FileByRoleType[] = await api.files.getFilesByRole(
        boardId,
        role.id,
        { signal }
      );
      runInAction(() => {
        this.filesByRole.set(role.id, files);
        this.collectionFileByRoleStore.get(role.id)?.setFirstLoaded(true);
      });

      return files;
    } catch (error) {
      if (axios.isCancel(error)) {
        return Promise.reject();
      }

      return Promise.reject(error);
    }
  };

  public fetchDashboard = async (
    fileId: number | string
  ): Promise<IDashboard> => {
    try {
      this.resetState();
      const dashboard: IDashboard = await api.files.getDashboard(fileId);

      this.setRoles(dashboard.current.roleQueue);
      this.setDashboard(dashboard);
      this.createCollectionFileByRoleStore(dashboard.current.roleQueue);

      await this.settingUpNavigationPath(dashboard);

      return dashboard;
    } catch (error) {
      return Promise.reject(error);
    }
  };

  public refreshDashboard = async (boardId: string | number) => {
    try {
      const dashboard: IDashboard = await api.files.getDashboard(boardId);

      this.setDashboard(dashboard);
      this.refreshCollectionFileByRoleStore(dashboard.current.roleQueue);
      this.setRoles(dashboard.current.roleQueue);

      const files = dashboard.current.roleQueue.map((role) => {
        //TODO: Add Filter for board
        // const filesStore = this.collectionFileByRoleStore.get(role.id);
        // const filter = filesStore.filer

        return this.fetchFilesByRole(role);
      });
      await Promise.all(files);

      return dashboard;
    } catch (error) {
      return Promise.reject(error);
    }
  };

  //#endregion
}

export default DashboardStore;
