import React from "react";
import { withRouter } from "react-router";
import { connect } from "react-redux";
import { ReactSVG } from 'react-svg'
import { withTranslation } from "react-i18next";
import isEqual from "lodash/isEqual";
import styled from "styled-components";
import {
  IconButton,
  Row,
  RowContainer,
  toastr,
  Link
} from "asc-web-components";
import EmptyFolderContainer from "./EmptyFolderContainer";
import FilesRowContent from "./FilesRowContent";
import { api, constants } from 'asc-web-common';
import {
  deleteFile,
  deleteFolder,
  deselectFile,
  fetchFiles,
  fetchFolder,
  //fetchRootFolders,
  selectFile,
  setAction,
  setTreeFolders
} from '../../../../../store/files/actions';
import { isFileSelected, getFileIcon, getFolderIcon, getFolderType } from '../../../../../store/files/selectors';
import store from "../../../../../store/store";
//import { getFilterByLocation } from "../../../../../helpers/converters";
//import config from "../../../../../../package.json";

const { FilesFilter } = api;
const { FileAction } = constants;

const linkStyles = { isHovered: true, type: "action", fontSize: "14px", className: "empty-folder_link", display: "flex" };

class SectionBodyContent extends React.PureComponent {
  constructor(props) {
    super(props);

    this.state = {
      editingId: null
    };
  }

  componentDidMount() {
    //const { fetchFiles } = this.props;

    //TODO: use right algorithm, fix fetching in src/index.html

    // var re = new RegExp(`${config.homepage}((/?)$|/filter)`, "gm");
    // const match = window.location.pathname.match(re);

    // if (match && match.length > 0) {
    //   const newFilter = getFilterByLocation(window.location);
    //   if (newFilter) {
    //     return fetchFiles(newFilter)
    //       .catch(error => toastr.error(error));
    //   } else {
    //     const filter = FilesFilter.getDefault();

    //     fetchFiles(filter)
    //       .catch(error => toastr.error(error));
    //   }
    // }
  }

  onClickRename = (item) => {
    const { id, fileExst } = item;

    this.setState({ editingId: id }, () => {
      this.props.setAction(
        {
          type: FileAction.Rename,
          extension: fileExst,
          id
        }
      );
    });
  };

  onEditComplete = item => {
    const { folderId, fileAction, filter, treeFolders, setTreeFolders, onLoading } = this.props;

    if (fileAction.type === FileAction.Create || fileAction.type === FileAction.Rename) {
      onLoading(true);
      fetchFiles(folderId, filter, store.dispatch).then(data => {
        if (!item.fileExst) {
          const path = data.selectedFolder.pathParts;
          const newTreeFolders = treeFolders;
          const folders = data.selectedFolder.folders;
          this.loop(path, newTreeFolders, item.parentId, folders);
          setTreeFolders(newTreeFolders);
        }
      }).finally(() => onLoading(false))
    }

    this.setState({ editingId: null }, () => {
      this.props.setAction({
        type: null
      });
    })
  }

  onClickDelete = (item) => {
    item.fileExst
      ? this.onDeleteFile(item.id, item.folderId)
      : this.onDeleteFolder(item.id, item.parentId);
  }

  onDeleteFile = (fileId, currentFolderId) => {
    const { deleteFile, filter } = this.props;

    deleteFile(fileId)
      .catch(err => toastr.error(err))
      .then(() => fetchFiles(currentFolderId, filter, store.dispatch))
      .then(() => toastr.success(`File moved to recycle bin`));
  }

  loop = (path, item, folderId, folders, foldersCount) => {
    const newPath = path;
    while (path.length !== 0) {
      const newItems = item.find(x => x.id === path[0]);
      newPath.shift();
      if (path.length === 0) {
        const currentItem = item.find(x => x.id === folderId);
        currentItem.folders = folders;
        currentItem.foldersCount = foldersCount;
        return;
      }
      this.loop(newPath, newItems.folders, folderId, folders, foldersCount);
    }
  };

  onDeleteFolder = (folderId, currentFolderId) => {
    const { deleteFolder, filter, treeFolders, setTreeFolders, onLoading } = this.props;
    onLoading(true);
    deleteFolder(folderId, currentFolderId)
      .catch(err => toastr.error(err))
      .then(() =>
        fetchFiles(currentFolderId, filter, store.dispatch).then(data => {
          const path = data.selectedFolder.pathParts;
          const newTreeFolders = treeFolders;
          const folders = data.selectedFolder.folders;
          const foldersCount = data.selectedFolder.foldersCount;
          this.loop(path, newTreeFolders, currentFolderId, folders, foldersCount);
          setTreeFolders(newTreeFolders);
        })
      )
      .then(() => toastr.success(`Folder moved to recycle bin`))
      .finally(() => onLoading(false));
  }

  onClickShare = item => {
    return false;
  }

  onClickLinkForPortal = item => {
    return fetchFolder(item.folderId, store.dispatch);
  }

  onClickDownload = item => {
    return window.open(item.viewUrl, "_blank");
  }

  onClickLinkEdit = item => {
    return false;
  }

  getFilesContextOptions = (item, viewer) => {
    const isFile = !!item.fileExst;

    const menu = [
      {
        key: "sharing-settings",
        label: "Sharing settings",
        onClick: this.onClickShare.bind(this, item),
        disabled: true
      },
      isFile
        ? {
          key: "send-by-email",
          label: "Send by e-mail",
          onClick: () => { },
          disabled: true
        }
        : null,
      {
        key: "link-for-portal-users",
        label: "Link for portal users",
        onClick: this.onClickLinkForPortal.bind(this, item),
        disabled: true
      },
      {
        key: "sep",
        isSeparator: true
      },
      isFile
        ? {
          key: "edit",
          label: "Edit",
          onClick: this.onClickLinkEdit.bind(this, item),
          disabled: true
        }
        : null,
      isFile
        ? {
          key: "download",
          label: "Download",
          onClick: this.onClickDownload.bind(this, item),
          disabled: false
        }
        : null,
      {
        key: "rename",
        label: "Rename",
        onClick: this.onClickRename.bind(this, item),
        disabled: false
      },
      {
        key: "delete",
        label: "Delete",
        onClick: this.onClickDelete.bind(this, item),
        disabled: false
      },
    ];

    return menu;
  };

  needForUpdate = (currentProps, nextProps) => {
    if (currentProps.checked !== nextProps.checked) {
      return true;
    }
    if (currentProps.editing !== nextProps.editing) {
      return true;
    }
    if (!isEqual(currentProps.data, nextProps.data)) {
      return true;
    }
    return false;
  };

  onContentRowSelect = (checked, file) => {

    if (!file) return;

    if (checked) {
      this.props.selectFile(file);
    } else {
      this.props.deselectFile(file);
    }
  };

  getItemIcon = (item, isEdit) => {
    const extension = item.fileExst;
    const icon = extension
      ? getFileIcon(extension, 24)
      : getFolderIcon(item.providerKey, 24);
    const loader = <div style={{width: '24px'}}></div> 

    return <ReactSVG
      beforeInjection={svg => {
        svg.setAttribute('style', 'margin-top: 4px');
        isEdit && svg.setAttribute('style', 'margin-left: 24px');
      }}
      src={icon}
      loading={() => loader}
    />;
  };

  onCreate = (format) => {
    this.props.setAction({
      type: FileAction.Create,
      extension: format,
      id: -1,
    });
  };

  onResetFilter = () => {
    const { selectedFolderId, onLoading } = this.props;
    onLoading(true);
    const newFilter = FilesFilter.getDefault();
    fetchFiles(selectedFolderId, newFilter, store.dispatch).catch(err =>
      toastr.error(err)
    ).finally(() => onLoading(false));
  }

  onGoToMyDocuments = () => {
    const { filter, myDocumentsId, onLoading } = this.props;
    const newFilter = filter.clone();
    onLoading(true);
    fetchFiles(myDocumentsId, newFilter, store.dispatch).finally(() =>
      onLoading(false)
    );
  };

  onBackToParentFolder = () => {
    const { filter, parentId, onLoading } = this.props;
    const newFilter = filter.clone();
    onLoading(true);
    fetchFiles(parentId, newFilter, store.dispatch).finally(() =>
      onLoading(false)
    );
  };

  renderEmptyRootFolderContainer = () => {
    const { currentFolderType, title, t } = this.props;
    const subheadingText = t("SubheadingEmptyText");
    const myDescription = t("MyEmptyContainerDescription");
    const shareDescription = t("SharedEmptyContainerDescription");
    const commonDescription = t("CommonEmptyContainerDescription");
    const trashDescription = t("TrashEmptyContainerDescription");

    const commonButtons = (
      <>
        <div className="empty-folder_container-links">
          <Link
            className="empty-folder_container_plus-image"
            color="#83888d"
            fontSize="26px"
            fontWeight="800"
            noHover
            onClick={() => console.log("Create document click")}
          >
            +
          </Link>
          <Link onClick={this.onCreate.bind(this, "docx")} {...linkStyles}>
            {t("Document")},
          </Link>
          <Link onClick={this.onCreate.bind(this, "xlsx")} {...linkStyles}>
            {t("Spreadsheet")},
          </Link>
          <Link onClick={this.onCreate.bind(this, "pptx")} {...linkStyles}>
            {t("Presentation")}
          </Link>
        </div>
        <div className="empty-folder_container-links">
          <Link
            className="empty-folder_container_plus-image"
            color="#83888d"
            fontSize="26px"
            fontWeight="800"
            onClick={this.onCreate.bind(this, null)}
            noHover
          >
            +
          </Link>
          <Link {...linkStyles} onClick={this.onCreate.bind(this, null)}>
            {t("Folder")}
          </Link>
        </div>
      </>
    );

    const trashButtons = (
      <div className="empty-folder_container-links">
        <img
          className="empty-folder_container_up-image"
          src="images/empty_screen_people.svg"
          alt=""
          onClick={this.onGoToMyDocuments}
        />
        <Link onClick={this.onGoToMyDocuments} {...linkStyles}>
          {t("GoToMyButton")}
        </Link>
      </div>
    );

    switch (currentFolderType) {
      case "My":
        return (
          <EmptyFolderContainer
            headerText={title}
            subheadingText={subheadingText}
            descriptionText={myDescription}
            imageSrc="images/empty_screen.png"
            buttons={commonButtons}
          />
        );
      case "Share":
        return (
          <EmptyFolderContainer
            headerText={title}
            subheadingText={subheadingText}
            descriptionText={shareDescription}
            imageSrc="images/empty_screen_forme.png"
          />
        );
      case "Common":
        return (
          <EmptyFolderContainer
            headerText={title}
            subheadingText={subheadingText}
            descriptionText={commonDescription}
            imageSrc="images/empty_screen_corporate.png"
            buttons={commonButtons}
          />
        );
      case "Trash":
        return (
          <EmptyFolderContainer
            headerText={title}
            subheadingText={subheadingText}
            descriptionText={trashDescription}
            imageSrc="images/empty_screen_trash.png"
            buttons={trashButtons}
          />
        );
      default:
        return;
    }
  };

  renderEmptyFolderContainer = () => {
    const { t } = this.props;
    const buttons = (
      <>
        <div className="empty-folder_container-links">
          <Link
            className="empty-folder_container_plus-image"
            color="#83888d"
            fontSize="26px"
            fontWeight="800"
            noHover
            onClick={() => console.log("Create document click")}
          >
            +
          </Link>
          <Link onClick={this.onCreate.bind(this, "docx")} {...linkStyles}>
            {t("Document")},
          </Link>
          <Link onClick={this.onCreate.bind(this, "xlsx")} {...linkStyles}>
            {t("Spreadsheet")},
          </Link>
          <Link onClick={this.onCreate.bind(this, "pptx")} {...linkStyles}>
            {t("Presentation")}
          </Link>
        </div>
        <div className="empty-folder_container-links">
          <Link
            className="empty-folder_container_plus-image"
            color="#83888d"
            fontSize="26px"
            fontWeight="800"
            onClick={this.onCreate.bind(this, null)}
            noHover
          >
            +
          </Link>
          <Link {...linkStyles} onClick={this.onCreate.bind(this, null)}>
            {t("Folder")}
          </Link>
        </div>
        <div className="empty-folder_container-links">
          <img
            className="empty-folder_container_up-image"
            src="images/up.svg"
            onClick={this.onBackToParentFolder}
            alt=""
          />
          <Link onClick={this.onBackToParentFolder} {...linkStyles}>
            {t("BackToParentFolderButton")}
          </Link>
        </div>
      </>
    );

    return (
      <EmptyFolderContainer
        headerText={t("EmptyFolderHeader")}
        imageSrc="images/empty_screen.png"
        buttons={buttons}
      />
    );
  };

  renderEmptyFilterContainer = () => {
    const { t } = this.props;
    const subheadingText = t("EmptyFilterSubheadingText");
    const descriptionText = t("EmptyFilterDescriptionText");

    const buttons = (
      <div className="empty-folder_container-links">
        <IconButton
          className="empty-folder_container-icon"
          size="12"
          onClick={this.onResetFilter}
          iconName="CrossIcon"
          isFill
          color="A3A9AE"
        />
        <Link onClick={this.onResetFilter} {...linkStyles}>
          {this.props.t("ClearButton")}
        </Link>
      </div>
    );

    return (
      <EmptyFolderContainer
        headerText={t("Filter")}
        subheadingText={subheadingText}
        descriptionText={descriptionText}
        imageSrc="images/empty_screen_filter.png"
        buttons={buttons}
      />
    )
  }

  render() {
    const {
      files,
      folders,
      viewer,
      parentId,
      folderId,
      settings,
      selection,
      fileAction,
      onLoading,
      currentFolderCount,
    } = this.props;
    const { editingId } = this.state;

    let items = [...folders, ...files];

    const SimpleFilesRow = styled(Row)`
      ${(props) =>
        !props.contextOptions &&
        `
          & > div:last-child {
              width: 0px;
            }
        `}
    `;

    if (fileAction && fileAction.type === FileAction.Create) {
      items.unshift({
        id: -1,
        title: "",
        parentId: folderId,
        fileExst: fileAction.extension,
      });
    }

    return currentFolderCount === 0 ? (
      parentId === 0 ? (
        this.renderEmptyRootFolderContainer()
      ) : (
          this.renderEmptyFolderContainer()
        )
    ) : items.length === 0 ? (
      this.renderEmptyFilterContainer()
    ) : (
          <RowContainer useReactWindow={false}>
            {items.map((item) => {
              const isEdit =
                fileAction.type &&
                (editingId === item.id || item.id === -1) &&
                item.fileExst === fileAction.extension;
              const contextOptions = this.getFilesContextOptions(
                item,
                viewer
              ).filter((o) => o);
              const contextOptionsProps =
                !contextOptions.length || isEdit ? {} : { contextOptions };
              const checked = isFileSelected(selection, item.id, item.parentId);
              const checkedProps = /* isAdmin(viewer) */ isEdit ? {} : { checked };
              const element = this.getItemIcon(item, isEdit);

              return (
                <SimpleFilesRow
                  key={item.id}
                  data={item}
                  element={element}
                  onSelect={this.onContentRowSelect}
                  editing={editingId}
                  {...checkedProps}
                  {...contextOptionsProps}
                  needForUpdate={this.needForUpdate}
                >
                  <FilesRowContent
                    item={item}
                    viewer={viewer}
                    culture={settings.culture}
                    onEditComplete={this.onEditComplete.bind(this, item)}
                    onLoading={onLoading}
                  />
                </SimpleFilesRow>
              );
            })}
          </RowContainer>
        );
  }
}

SectionBodyContent.defaultProps = {
  files: null
};

const mapStateToProps = state => {
  const { selectedFolder, treeFolders } = state.files;
  const { id, title, foldersCount, filesCount } = selectedFolder;
  const currentFolderType = getFolderType(id, treeFolders);

  const myFolderIndex = 0;
  const currentFolderCount = filesCount + foldersCount;

  return {
    fileAction: state.files.fileAction,
    files: state.files.files,
    filter: state.files.filter,
    folderId: state.files.selectedFolder.id,
    folders: state.files.folders,
    parentId: state.files.selectedFolder.parentId,
    selected: state.files.selected,
    selection: state.files.selection,
    settings: state.auth.settings,
    viewer: state.auth.user,
    treeFolders: state.files.treeFolders,
    currentFolderType,
    title,
    myDocumentsId: treeFolders[myFolderIndex].id,
    currentFolderCount,
    selectedFolderId: id
  };
};

export default connect(
  mapStateToProps,
  {
    deleteFile,
    deleteFolder,
    deselectFile,
    fetchFiles,
    //fetchRootFolders,
    selectFile,
    setAction,
    setTreeFolders
  }
)(withRouter(withTranslation()(SectionBodyContent)));
