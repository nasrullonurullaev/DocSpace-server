import React from "react";
import { useLocation, Outlet } from "react-router-dom";
import { isMobile } from "react-device-detect";
import { observer, inject } from "mobx-react";
import { withTranslation } from "react-i18next";

import { showLoader, hideLoader } from "@docspace/common/utils";

import Section from "@docspace/common/components/Section";

import DragTooltip from "SRC_DIR/components/DragTooltip";

import {
  SectionFilterContent,
  SectionHeaderContent,
  SectionPagingContent,
} from "./Section";
import AccountsDialogs from "./Section/AccountsBody/Dialogs";

import MediaViewer from "./MediaViewer";
import SelectionArea from "./SelectionArea";
import { InfoPanelBodyContent, InfoPanelHeaderContent } from "./InfoPanel";
import { RoomSearchArea } from "@docspace/common/constants";

import {
  useFiles,
  useSDK,
  useOperations,
  useAccounts,
  useSettings,
  useDashboard,
} from "./Hooks";

const PureHome = (props) => {
  const {
    fetchFiles,
    fetchRooms,

    //homepage,
    setIsLoading,

    setToPreviewFile,
    playlist,

    getFileInfo,
    gallerySelected,
    setIsUpdatingRowItem,
    setIsPreview,
    selectedFolderStore,
    t,
    startUpload,
    setDragging,
    dragging,
    uploadEmptyFolders,
    disableDrag,
    uploaded,
    converted,
    setUploadPanelVisible,
    clearPrimaryProgressData,
    primaryProgressDataVisible,
    isProgressFinished,
    secondaryProgressDataStoreIcon,
    itemsSelectionLength,
    itemsSelectionTitle,
    setItemsSelectionTitle,
    refreshFiles,

    setFrameConfig,
    user,
    folders,
    files,
    selection,
    filesList,
    removeFirstUrl,

    createFile,
    createFolder,
    createRoom,

    setViewAs,
    viewAs,

    firstLoad,

    isPrivacyFolder,
    isRecycleBinFolder,
    isErrorRoomNotAvailable,

    primaryProgressDataPercent,
    primaryProgressDataIcon,
    primaryProgressDataAlert,
    clearUploadedFilesHistory,

    secondaryProgressDataStoreVisible,
    secondaryProgressDataStorePercent,

    secondaryProgressDataStoreAlert,

    tReady,
    isFrame,
    showTitle,
    showFilter,
    frameConfig,
    withPaging,
    isEmptyPage,

    setPortalTariff,

    accountsViewAs,
    fetchPeople,
    setSelectedNode,
    onClickBack,

    showFilterLoader,
    fetchDashboard,
    setCategoryType,
    dashboardViewAs,
  } = props;

  const location = useLocation();

  const isAccountsPage = location.pathname.includes("/accounts/filter");
  const isSettingsPage = location.pathname.includes("settings");
  const isDashboardPage = location.pathname.includes("dashboard");

  useDashboard({
    isDashboardPage,
    setIsLoading,
    fetchDashboard,
    setCategoryType,
  });

  const { onDrop } = useFiles({
    t,
    dragging,
    setDragging,
    disableDrag,
    uploadEmptyFolders,
    startUpload,
    fetchFiles,
    fetchRooms,
    setIsLoading,

    isAccountsPage,
    isSettingsPage,
    isDashboardPage,

    location,

    playlist,

    getFileInfo,
    setToPreviewFile,
    setIsPreview,

    setIsUpdatingRowItem,
    removeFirstUrl,

    gallerySelected,
  });

  const { showUploadPanel } = useOperations({
    t,
    setUploadPanelVisible,
    primaryProgressDataVisible,
    uploaded,
    converted,
    clearPrimaryProgressData,
    isProgressFinished,
    refreshFiles,
    itemsSelectionTitle,
    secondaryProgressDataStoreIcon,
    itemsSelectionLength,
    isAccountsPage,
    isSettingsPage,
    setItemsSelectionTitle,
  });

  useAccounts({
    t,
    isAccountsPage,
    location,

    setIsLoading,

    setSelectedNode,
    fetchPeople,
    setPortalTariff,
  });

  useSettings({ t, isSettingsPage, setIsLoading });

  useSDK({
    frameConfig,
    setFrameConfig,
    selectedFolderStore,
    folders,
    files,
    filesList,
    selection,
    user,
    createFile,
    createFolder,
    createRoom,
    refreshFiles,
    setViewAs,
  });

  React.useEffect(() => {
    window.addEventListener("popstate", onClickBack);

    return () => {
      window.removeEventListener("popstate", onClickBack);
    };
  }, []);

  let sectionProps = {};

  if (isSettingsPage) {
    sectionProps.isInfoPanelAvailable = false;
    sectionProps.viewAs = "settings";
  } else {
    sectionProps = {
      withPaging,
      withBodyScroll: true,
      withBodyAutoFocus: !isMobile,
      firstLoad,
      isLoaded: !firstLoad,
      viewAs: isDashboardPage ? dashboardViewAs : accountsViewAs,
    };

    if (!isAccountsPage && !isDashboardPage) {
      sectionProps.dragging = dragging;
      sectionProps.uploadFiles = true;
      sectionProps.onDrop =
        isRecycleBinFolder || isPrivacyFolder ? null : onDrop;

      sectionProps.clearUploadedFilesHistory = clearUploadedFilesHistory;
      sectionProps.viewAs = viewAs;
      sectionProps.hideAside =
        primaryProgressDataVisible || secondaryProgressDataStoreVisible;

      sectionProps.isEmptyPage = isEmptyPage;
    }
  }

  sectionProps.onOpenUploadPanel = showUploadPanel;
  sectionProps.showPrimaryProgressBar = primaryProgressDataVisible;
  sectionProps.primaryProgressBarValue = primaryProgressDataPercent;
  sectionProps.primaryProgressBarIcon = primaryProgressDataIcon;
  sectionProps.showPrimaryButtonAlert = primaryProgressDataAlert;
  sectionProps.showSecondaryProgressBar = secondaryProgressDataStoreVisible;
  sectionProps.secondaryProgressBarValue = secondaryProgressDataStorePercent;
  sectionProps.secondaryProgressBarIcon = secondaryProgressDataStoreIcon;
  sectionProps.showSecondaryButtonAlert = secondaryProgressDataStoreAlert;

  return (
    <>
      {isSettingsPage || isDashboardPage ? (
        <></>
      ) : isAccountsPage ? (
        <AccountsDialogs />
      ) : (
        <>
          <DragTooltip />
          <SelectionArea />
        </>
      )}
      <MediaViewer />
      <Section {...sectionProps}>
        {(!isErrorRoomNotAvailable || isAccountsPage || isSettingsPage) && (
          <Section.SectionHeader>
            {isFrame ? (
              showTitle && <SectionHeaderContent />
            ) : (
              <SectionHeaderContent />
            )}
          </Section.SectionHeader>
        )}

        {(((!isEmptyPage || showFilterLoader) && !isErrorRoomNotAvailable) ||
          isAccountsPage) &&
          !isSettingsPage && (
            <Section.SectionFilter>
              {isFrame ? (
                showFilter && <SectionFilterContent />
              ) : (
                <SectionFilterContent />
              )}
            </Section.SectionFilter>
          )}

        <Section.SectionBody>
          <Outlet />
        </Section.SectionBody>

        <Section.InfoPanelHeader>
          <InfoPanelHeaderContent />
        </Section.InfoPanelHeader>
        <Section.InfoPanelBody>
          <InfoPanelBodyContent />
        </Section.InfoPanelBody>

        {withPaging && !isSettingsPage && (
          <Section.SectionPaging>
            <SectionPagingContent tReady={tReady} />
          </Section.SectionPaging>
        )}
      </Section>
    </>
  );
};

const Home = withTranslation(["Files", "People"])(PureHome);

export default inject(
  ({
    auth,
    filesStore,
    uploadDataStore,
    treeFoldersStore,
    mediaViewerDataStore,
    peopleStore,
    filesActionsStore,
    oformsStore,
    selectedFolderStore,
    clientLoadingStore,
    dashboardStore,
  }) => {
    const {
      secondaryProgressDataStore,
      primaryProgressDataStore,
      clearUploadedFilesHistory,
    } = uploadDataStore;

    const { fetchDashboard, viewAs: dashboardViewAs } = dashboardStore;

    const {
      firstLoad,
      setIsSectionBodyLoading,
      setIsSectionFilterLoading,
      isLoading,

      showFilterLoader,
    } = clientLoadingStore;

    const setIsLoading = (param, withTimer = true) => {
      setIsSectionFilterLoading(param, withTimer);
      setIsSectionBodyLoading(param, withTimer);
    };

    const {
      fetchFiles,
      fetchRooms,
      setCategoryType,
      selection,
      dragging,
      setDragging,

      viewAs,
      getFileInfo,
      setIsUpdatingRowItem,

      folders,
      files,
      filesList,

      createFile,
      createFolder,
      createRoom,
      refreshFiles,
      setViewAs,
      isEmptyPage,

      disableDrag,
      isErrorRoomNotAvailable,
      setIsPreview,
    } = filesStore;

    const { gallerySelected } = oformsStore;

    const {
      isRecycleBinFolder,
      isPrivacyFolder,

      setExpandedKeys,
      isRoomsFolder,
      isArchiveFolder,
      setSelectedNode,
    } = treeFoldersStore;

    const {
      visible: primaryProgressDataVisible,
      percent: primaryProgressDataPercent,
      icon: primaryProgressDataIcon,
      alert: primaryProgressDataAlert,
      clearPrimaryProgressData,
    } = primaryProgressDataStore;

    const {
      visible: secondaryProgressDataStoreVisible,
      percent: secondaryProgressDataStorePercent,
      icon: secondaryProgressDataStoreIcon,
      alert: secondaryProgressDataStoreAlert,
      isSecondaryProgressFinished: isProgressFinished,
      itemsSelectionLength,
      itemsSelectionTitle,
      setItemsSelectionTitle,
    } = secondaryProgressDataStore;

    const { setUploadPanelVisible, startUpload, uploaded, converted } =
      uploadDataStore;

    const { uploadEmptyFolders, onClickBack } = filesActionsStore;

    const selectionLength = isProgressFinished ? selection.length : null;
    const selectionTitle = isProgressFinished
      ? filesStore.selectionTitle
      : null;

    const { setToPreviewFile, playlist, removeFirstUrl } = mediaViewerDataStore;

    const { settingsStore, currentTariffStatusStore } = auth;

    const { setPortalTariff } = currentTariffStatusStore;

    const { setFrameConfig, frameConfig, isFrame, withPaging, showCatalog } =
      settingsStore;

    const {
      usersStore,

      viewAs: accountsViewAs,
    } = peopleStore;

    const { getUsersList: fetchPeople } = usersStore;

    if (!firstLoad) {
      if (isLoading) {
        showLoader();
      } else {
        hideLoader();
      }
    }

    return {
      //homepage: config.homepage,
      firstLoad,
      dragging,
      viewAs,
      uploaded,
      converted,
      isRecycleBinFolder,
      isPrivacyFolder,
      isVisitor: auth.userStore.user.isVisitor,

      primaryProgressDataVisible,
      primaryProgressDataPercent,
      primaryProgressDataIcon,
      primaryProgressDataAlert,
      clearPrimaryProgressData,

      clearUploadedFilesHistory,

      secondaryProgressDataStoreVisible,
      secondaryProgressDataStorePercent,
      secondaryProgressDataStoreIcon,
      secondaryProgressDataStoreAlert,

      selectionLength,
      isProgressFinished,
      selectionTitle,

      itemsSelectionLength,
      setItemsSelectionTitle,
      itemsSelectionTitle,
      isErrorRoomNotAvailable,
      isRoomsFolder,
      isArchiveFolder,

      disableDrag,

      setExpandedKeys,

      setDragging,
      setIsLoading,
      fetchFiles,
      fetchRooms,

      setUploadPanelVisible,
      startUpload,
      uploadEmptyFolders,

      setToPreviewFile,
      setIsPreview,
      playlist,
      removeFirstUrl,

      getFileInfo,
      gallerySelected,
      setIsUpdatingRowItem,

      setFrameConfig,
      frameConfig,
      isFrame,
      showTitle: frameConfig?.showTitle,
      showFilter: frameConfig?.showFilter,
      user: auth.userStore.user,
      folders,
      files,
      selection,
      filesList,
      selectedFolderStore,
      createFile,
      createFolder,
      createRoom,
      refreshFiles,
      setViewAs,
      withPaging,
      isEmptyPage,

      setPortalTariff,

      accountsViewAs,
      fetchPeople,
      setSelectedNode,
      onClickBack,

      showFilterLoader,
      fetchDashboard,
      setCategoryType,
      dashboardViewAs,
    };
  }
)(observer(Home));
