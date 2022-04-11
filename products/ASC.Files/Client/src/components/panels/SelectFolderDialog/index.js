import React from "react";
import { inject, observer } from "mobx-react";
import { withTranslation } from "react-i18next";
import PropTypes from "prop-types";
import throttle from "lodash/throttle";
import { getCommonThirdPartyList } from "@appserver/common/api/settings";
import { getFolder } from "@appserver/common/api/files";
import SelectFolderDialogAsideView from "./AsideView";
import utils from "@appserver/components/utils";
import toastr from "studio/toastr";
import SelectionPanel from "../SelectionPanel/SelectionPanelBody";
import { FilterType } from "@appserver/common/constants";

const { desktop } = utils.device;

class SelectFolderModalDialog extends React.Component {
  constructor(props) {
    super(props);
    const { id, displayType, filter } = this.props;
    this.newFilter = filter.clone();
    this.newFilter.filterType = FilterType.FilesOnly;

    this.state = {
      isLoadingData: false,
      isInitialLoader: false,
      folderId: id ? id : "",
      displayType: displayType || this.getDisplayType(),
      canCreate: true,
      isAvailable: true,
      filesList: [],

      isNextPageLoading: false,
      page: 0,
      hasNextPage: true,
      files: [],
    };
    this.throttledResize = throttle(this.setDisplayType, 300);
    this.noTreeSwitcher = false;
  }

  async componentDidMount() {
    const {
      treeFolders,
      foldersType,
      id,
      onSetBaseFolderPath,
      onSelectFolder,
      foldersList,
      treeFromInput,
      setSelectedNode,
      setSelectedFolder,
      setExpandedPanelKeys,
      displayType,
    } = this.props;

    !displayType && window.addEventListener("resize", this.throttledResize);

    let timerId = setTimeout(() => {
      this.setState({ isInitialLoader: true });
    }, 1000);

    let resultingFolderTree, resultingId;

    try {
      [
        resultingFolderTree,
        resultingId,
      ] = await SelectionPanel.getBasicFolderInfo(
        treeFolders,
        foldersType,
        id,
        onSetBaseFolderPath,
        onSelectFolder,
        foldersList,
        true,
        setSelectedNode,
        setSelectedFolder,
        setExpandedPanelKeys
      );

      clearTimeout(timerId);
      timerId = null;
    } catch (e) {
      toastr.error(e);

      clearTimeout(timerId);
      timerId = null;
      this.setState({ isInitialLoader: false });

      return;
    }

    const tree = treeFromInput ? treeFromInput : resultingFolderTree;

    if (tree.length === 0) {
      this.setState({ isAvailable: false });
      onSelectFolder(null);
      return;
    }
    const resId = treeFromInput ? id : resultingId;

    onSelectFolder && onSelectFolder(resId);

    this.setState({
      resultingFolderTree: tree,
      isInitialLoader: false,

      folderId: resId,
    });
  }

  componentDidUpdate(prevProps) {
    const {
      storeFolderId,
      canCreate,
      checkPossibilityCreating,
      isReset,
    } = this.props;

    if (checkPossibilityCreating && storeFolderId !== prevProps.storeFolderId) {
      clearTimeout(this.timerId);
      this.timerId = null;

      this.setState({
        canCreate: canCreate,
      });
    }

    if (isReset && isReset !== prevProps.isReset) {
      this.onResetInfo();
    }
  }

  componentWillUnmount() {
    const { setExpandedPanelKeys, setSelectedFolder } = this.props;

    clearTimeout(this.timerId);
    this.timerId = null;

    if (this.throttledResize) {
      this.throttledResize && this.throttledResize.cancel();
      window.removeEventListener("resize", this.throttledResize);
    }

    setExpandedPanelKeys(null);
    setSelectedFolder(null);
  }
  getDisplayType = () => {
    const displayType =
      window.innerWidth < desktop.match(/\d+/)[0] ? "aside" : "modal";

    return displayType;
  };

  setDisplayType = () => {
    const displayType = this.getDisplayType();

    this.setState({ displayType: displayType });
  };

  onSelect = async (folder) => {
    const {
      setSelectedNode,
      setExpandedPanelKeys,
      setSelectedFolder,
      checkPossibilityCreating,
    } = this.props;
    const { folderId } = this.state;

    if (+folderId === +folder[0]) return;

    this.setState({
      folderId: folder[0],
      files: [],
      hasNextPage: true,
      page: 0,
    });

    if (checkPossibilityCreating) {
      this.timerId = setTimeout(
        () =>
          this.setState({
            canCreate: false,
          }),
        100
      );
    }
    const isFilesModule =
      window.location.href.indexOf("products/files") !== -1 &&
      window.location.href.indexOf("doceditor") === -1;

    !isFilesModule &&
      SelectionPanel.setFolderObjectToTree(
        folder[0],
        setSelectedNode,
        setExpandedPanelKeys,
        setSelectedFolder
      );
  };

  onButtonClick = (e) => {
    const { onClose, onSave, onSetNewFolderPath, onSelectFolder } = this.props;
    const { folderId } = this.state;

    onSave && onSave(e, folderId);
    onSetNewFolderPath && onSetNewFolderPath(folderId);
    onSelectFolder && onSelectFolder(folderId);

    onClose && onClose();
  };

  onResetInfo = async () => {
    const {
      id,
      setSelectedNode,
      setExpandedPanelKeys,
      setSelectedFolder,
    } = this.props;

    SelectionPanel.setFolderObjectToTree(
      id,
      setSelectedNode,
      setExpandedPanelKeys,
      setSelectedFolder
    );

    this.setState({
      folderId: id,
    });
  };

  _loadNextPage = () => {
    const { files, page, folderId } = this.state;

    if (this._isLoadNextPage) return;

    this._isLoadNextPage = true;

    const pageCount = 30;
    this.newFilter.page = page;
    this.newFilter.pageCount = pageCount;

    this.setState({ isNextPageLoading: true }, async () => {
      try {
        const data = await getFolder(folderId, this.newFilter);

        const finalData = [...data.files];

        const newFilesList = [...files].concat(finalData);

        const hasNextPage = newFilesList.length < data.total - 1;

        this._isLoadNextPage = false;
        this.setState((state) => ({
          isDataLoading: false,
          hasNextPage: hasNextPage,
          isNextPageLoading: false,
          page: state.page + 1,
          files: newFilesList,
          ...(page === 0 && { folderTitle: data.current.title }),
        }));
      } catch (e) {
        toastr.error(e);
        this.setState({
          isDataLoading: false,
        });
      }
    });
  };
  render() {
    const {
      t,
      theme,
      isPanelVisible,
      zIndex,
      onClose,
      withoutProvider,
      isNeedArrowIcon,
      header,
      dialogName,
      footer,
      buttonName,
    } = this.props;
    const {
      folderId,
      displayType,
      canCreate,
      isLoadingData,
      isAvailable,
      resultingFolderTree,
      filesList,

      hasNextPage,
      isNextPageLoading,
      files,
      page,
      folderTitle,
    } = this.state;

    const primaryButtonName = buttonName ? buttonName : t("Common:SaveButton");

    return displayType === "aside" ? (
      <SelectFolderDialogAsideView
        theme={theme}
        t={t}
        isPanelVisible={isPanelVisible}
        zIndex={zIndex}
        onClose={onClose}
        withoutProvider={withoutProvider}
        isNeedArrowIcon={isNeedArrowIcon}
        certainFolders={true}
        folderId={folderId}
        resultingFolderTree={resultingFolderTree}
        onSelectFolder={this.onSelect}
        onButtonClick={this.onButtonClick}
        header={header}
        dialogName={dialogName}
        footer={footer}
        canCreate={canCreate}
        isLoadingData={isLoadingData}
        primaryButtonName={primaryButtonName}
        noTreeSwitcher={this.noTreeSwitcher}
        isAvailable={isAvailable}
      />
    ) : (
      <SelectionPanel
        t={t}
        theme={theme}
        isPanelVisible={isPanelVisible}
        onClose={onClose}
        withoutProvider={withoutProvider}
        folderId={folderId}
        resultingFolderTree={resultingFolderTree}
        onButtonClick={this.onButtonClick}
        header={header}
        dialogName={dialogName}
        footer={footer}
        canCreate={canCreate}
        isLoadingData={isLoadingData}
        primaryButtonName={primaryButtonName}
        noTreeSwitcher={this.noTreeSwitcher}
        isAvailable={isAvailable}
        onSelectFolder={this.onSelect}
        filesList={filesList}
        isNextPageLoading={isNextPageLoading}
        page={page}
        hasNextPage={hasNextPage}
        files={files}
        loadNextPage={this._loadNextPage}
        folderTitle={folderTitle}
        folderSelection
      />
    );
  }
}

SelectFolderModalDialog.propTypes = {
  onSelectFolder: PropTypes.func,
  onClose: PropTypes.func.isRequired,
  isPanelVisible: PropTypes.bool.isRequired,
  foldersType: PropTypes.oneOf([
    "common",
    "third-party",
    "exceptSortedByTags",
    "exceptPrivacyTrashFolders",
  ]),
  displayType: PropTypes.oneOf(["aside", "modal"]),
  id: PropTypes.oneOfType([PropTypes.string, PropTypes.number]),
  withoutProvider: PropTypes.bool,
  checkPossibilityCreating: PropTypes.bool,
};
SelectFolderModalDialog.defaultProps = {
  id: "",
  withoutProvider: false,
  checkPossibilityCreating: false,
};

const SelectFolderDialogWrapper = inject(
  ({
    treeFoldersStore,
    selectedFolderStore,
    selectedFilesStore,
    filesStore,
    auth,
  }) => {
    const {
      setSelectedNode,
      setExpandedPanelKeys,
      treeFolders,
    } = treeFoldersStore;

    const { canCreate, filter } = filesStore;
    const { setSelectedFolder, id } = selectedFolderStore;
    const { setFolderId } = selectedFilesStore;
    return {
      theme: auth.settingsStore.theme,
      setSelectedFolder,
      setSelectedNode,
      canCreate,
      storeFolderId: id,
      setExpandedPanelKeys,
      setFolderId,
      treeFolders,
      filter,
    };
  }
)(
  observer(
    withTranslation(["SelectFolder", "Common", "Translations"])(
      SelectFolderModalDialog
    )
  )
);

class SelectFolderDialog extends React.Component {
  static getCommonThirdPartyList = async () => {
    const commonThirdPartyArray = await getCommonThirdPartyList();

    commonThirdPartyArray.map((currentValue, index) => {
      commonThirdPartyArray[index].key = `0-${index}`;
    });

    return commonThirdPartyArray;
  };

  render() {
    return <SelectFolderDialogWrapper {...this.props} />;
  }
}

export default SelectFolderDialog;
