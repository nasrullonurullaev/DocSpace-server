import React from "react";
import ComboBox from "@appserver/components/combobox";
import { ShareAccessRights } from "@appserver/common/constants";
import DropDownItem from "@appserver/components/drop-down-item";
import AccessRightSelect from "@appserver/components/access-right-select";
import { getAccessIcon } from "../../../helpers/files-helpers";
import { ReactSVG } from "react-svg";

const {
  FullAccess,
  CustomFilter,
  Review,
  FormFilling,
  Comment,
  ReadOnly,
  DenyAccess,
} = ShareAccessRights;

const AccessComboBox = (props) => {
  const {
    access,
    accessOptions,
    directionY,
    directionX,
    isDisabled,
    itemId,
    onAccessChange,
    t,
    theme,
    disableLink,
    fixedDirection,
    canDelete,
    onRemoveUserClick,
    isExternalLink,
  } = props;

  const [isLoading, setIsLoading] = React.useState(true);
  const [availableOptions, setAvailableOptions] = React.useState([]);
  const [selectedOption, setSelectedOption] = React.useState(null);

  const ref = React.useRef(null);

  const onSelect = React.useCallback(
    (e) => {
      const access = +e.target.dataset.access;

      if (access) {
        const item = availableOptions.find((option) => {
          return option.dataAccess === access;
        });

        setSelectedOption(item);

        onAccessChange && onAccessChange(e);
      } else {
        onRemoveUserClick && onRemoveUserClick(e);
      }
    },
    [availableOptions, onAccessChange, onRemoveUserClick]
  );

  React.useEffect(() => {
    const accessRights = disableLink ? ReadOnly : access;

    const newAvailableOptions = [];
    accessOptions.forEach((option) => {
      switch (option) {
        case "FullAccess":
          const accessItem = {
            key: FullAccess,
            title: t("Common:FullAccess"),
            label: t("Common:FullAccess"),
            icon: "/static/images/access.edit.react.svg",
            itemId: itemId,
            dataAccess: FullAccess,
          };

          newAvailableOptions.push(accessItem);

          if (accessRights === FullAccess) {
            setSelectedOption(accessItem);
          }

          break;

        case "FilterEditing":
          const filterItem = {
            key: CustomFilter,
            title: t("CustomFilter"),
            label: t("CustomFilter"),
            icon: "/static/images/custom.filter.react.svg",
            itemId: itemId,
            dataAccess: CustomFilter,
          };

          newAvailableOptions.push(filterItem);

          if (accessRights === CustomFilter) {
            setSelectedOption(filterItem);
          }

          break;

        case "Review":
          const reviewItem = {
            key: Review,
            title: t("Common:Review"),
            label: t("Common:Review"),
            icon: "/static/images/access.review.react.svg",
            itemId: itemId,
            dataAccess: Review,
          };

          newAvailableOptions.push(reviewItem);

          if (accessRights === Review) {
            setSelectedOption(reviewItem);
          }

          break;

        case "FormFilling":
          const formItem = {
            key: FormFilling,
            title: t("FormFilling"),
            label: t("FormFilling"),
            icon: "/static/images/access.form.react.svg",
            itemId: itemId,
            dataAccess: FormFilling,
          };

          newAvailableOptions.push(formItem);

          if (accessRights === FormFilling) {
            setSelectedOption(formItem);
          }

          break;

        case "Comment":
          const commentItem = {
            key: Comment,
            title: t("Comment"),
            label: t("Comment"),
            icon: "/static/images/access.comment.react.svg",
            itemId: itemId,
            dataAccess: Comment,
          };

          newAvailableOptions.push(commentItem);

          if (accessRights === Comment) {
            setSelectedOption(commentItem);
          }

          break;

        case "ReadOnly":
          const readItem = {
            key: ReadOnly,
            title: t("ReadOnly"),
            label: t("ReadOnly"),
            icon: "/static/images/eye.react.svg",
            itemId: itemId,
            dataAccess: ReadOnly,
          };

          newAvailableOptions.push(readItem);

          if (accessRights === ReadOnly) {
            setSelectedOption(readItem);
          }

          break;

        case "DenyAccess":
          const denyItem = {
            key: DenyAccess,
            title: t("DenyAccess"),
            label: t("DenyAccess"),
            icon: "/static/images/access.none.react.svg",
            itemId: itemId,
            dataAccess: DenyAccess,
          };

          newAvailableOptions.push(denyItem);

          if (accessRights === DenyAccess) {
            setSelectedOption(denyItem);
          }

          break;
      }
    });

    if (canDelete) {
      newAvailableOptions.push({ key: "separator", isSeparator: true });
      newAvailableOptions.push({
        key: "delete",
        title: t("Common:Delete"),
        label: t("Common:Delete"),
        icon: "/static/images/delete.react.svg",
        dataFor: itemId,
        onClick: onRemoveUserClick,
      });
    }

    setAvailableOptions(newAvailableOptions);
    if (newAvailableOptions.length > 0) {
      setIsLoading(false);
    }
  }, [
    access,
    disableLink,
    accessOptions,
    onRemoveUserClick,
    itemId,
    canDelete,
  ]);

  const renderAdvancedOption = React.useCallback(() => {
    return (
      <>
        {availableOptions?.map((option) => (
          <DropDownItem
            key={option.key}
            label={option.label}
            icon={option.icon}
            data-id={option.itemId}
            data-access={option.dataAccess}
            data-for={option.dataFor}
            onClick={onSelect}
            isSeparator={option.isSeparator}
          />
        ))}
      </>
    );
  }, [availableOptions, onSelect]);

  const advancedOptions = renderAdvancedOption();

  return isLoading ? (
    <> </>
  ) : isExternalLink ? (
    <AccessRightSelect
      options={[]}
      selectedOption={selectedOption}
      advancedOptions={advancedOptions}
    />
  ) : (
    <ComboBox
      theme={theme}
      advancedOptions={advancedOptions}
      options={[]}
      selectedOption={{}}
      size="content"
      className="panel_combo-box"
      scaled={false}
      directionX={directionX}
      directionY={directionY}
      disableIconClick={false}
      isDisabled={isDisabled}
      isDefaultMode={true}
      ref={ref}
      forwardRef={ref}
      fixedDirection={fixedDirection}
    >
      <ReactSVG
        src={selectedOption.icon}
        className="sharing-access-combo-box-icon"
      />
    </ComboBox>
  );
};

export default React.memo(AccessComboBox);
