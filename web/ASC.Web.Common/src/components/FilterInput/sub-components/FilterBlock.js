import React from "react";
import FilterButton from "./FilterButton";
import HideFilter from "./HideFilter";
import throttle from "lodash/throttle";
import { ComboBox } from "asc-web-components";
import CloseButton from "./CloseButton";
import isEqual from "lodash/isEqual";
import PropTypes from "prop-types";
import {
  StyledFilterItem,
  StyledFilterItemContent,
  StyledCloseButtonBlock,
} from "../StyledFilterInput";
import GroupSelector from "../../GroupSelector";
import PeopleSelector from "../../PeopleSelector";

class FilterItem extends React.Component {
  constructor(props) {
    super(props);
    const { id, selectedItem, typeSelector } = props;

    const selectedOption =
      selectedItem &&
      selectedItem.key &&
      (typeSelector === selectedItem.type || id.includes(typeSelector))
        ? {
            key: selectedItem.key,
            label: selectedItem.label,
          }
        : {
            key: null,
            label: this.props.defaultSelectLabel,
            default: true,
          };

    const isOpenSelector = Boolean(selectedOption.key);
    this.state = {
      id,
      isOpen: false,
      isOpenSelector: !isOpenSelector,
      selectedOption,
      needUpdate: false,
    };
  }

  componentDidUpdate(prevProps, prevState) {
    const { selectedItem, defaultSelectLabel } = this.props;

    if (
      selectedItem &&
      selectedItem.key !== this.state.selectedOption.key &&
      selectedItem.key !== prevProps.selectedItem.key
    ) {
      const selectedOption = selectedItem.key
        ? {
            key: selectedItem.key,
            label: selectedItem.label,
          }
        : {
            key: null,
            label: defaultSelectLabel,
            default: true,
          };
      const isOpenSelector = Boolean(selectedOption.key);
      this.setState({
        isOpenSelector: !isOpenSelector,
        selectedOption,
      });
    }

    if (this.state.needUpdate) {
      this.props.onFilterRender();
      this.setNeedUpdate(false);
    }
  }

  onSelect = (option) => {
    const { group, key, label, inSubgroup } = option;
    this.props.onSelectFilterItem(null, {
      key: group + "_" + key,
      label,
      group,
      inSubgroup: !!inSubgroup,
    });
  };
  onClick = () => {
    const { isDisabled, id, onClose } = this.props;
    !isDisabled && onClose(id);
  };

  toggleCombobox = (e, isOpen) => this.setState({ isOpen });

  setNeedUpdate = (needUpdate) => {
    this.setState({
      needUpdate,
    });
  };

  onCancelSelector = (e) => {
    if (
      this.state.isOpenSelector &&
      (e.target.id === "filter-selector_button" ||
        e.target.closest("#filter-selector_button"))
    ) {
      // Skip double set of isOpen property
      return;
    }
    this.setState({ isOpenSelector: false });
  };

  onSelectGroup = (selected) => {
    const { key, label } = selected[0];
    const selectedOption = {
      key,
      label,
    };
    this.setState({
      selectedOption,
      isOpenSelector: false,
    });

    const {
      onSelectFilterItem,
      id,
      groupItems,
      typeSelector,
      defaultOption,
      groupsCaption,
      defaultOptionLabel,
      defaultSelectLabel,
    } = this.props;

    onSelectFilterItem(null, {
      isSelector: true,
      key: id,
      group: groupItems[0].group, //hack, replace it: this.props.id.slice(0, this.props.id.indexOf('_'));
      label: this.props.label,
      typeSelector,
      defaultOption,
      groupsCaption,
      defaultOptionLabel,
      defaultSelectLabel,
      selectedItem: selected[0],
    });
  };

  onPeopleSelectorClick = () =>
    this.setState({ isOpenSelector: !this.state.isOpenSelector });

  render() {
    const { id, isOpen, isOpenSelector, selectedOption } = this.state;
    const {
      block,
      opened,
      isDisabled,
      groupLabel,
      groupItems,
      label,
      typeSelector,
      defaultOptionLabel,
      groupsCaption,
      defaultOption,
    } = this.props;
    return (
      <StyledFilterItem key={id} id={id} block={block} opened={opened}>
        <StyledFilterItemContent isDisabled={isDisabled} isOpen={isOpen}>
          {typeSelector === "group" && (
            <>
              {groupLabel}:
              <ComboBox
                id="filter-selector_button"
                className="styled-combobox"
                options={[]}
                opened={isOpenSelector}
                selectedOption={selectedOption}
                size="content"
                toggleAction={this.onPeopleSelectorClick}
                displayType="toggle"
                scaled={false}
                noBorder={true}
                directionX="left"
                dropDownMaxHeight={200}
              ></ComboBox>
              <GroupSelector
                isOpen={isOpenSelector}
                isMultiSelect={false}
                onCancel={this.onCancelSelector}
                onSelect={this.onSelectGroup}
              />
            </>
          )}
          {typeSelector === "user" && (
            <>
              {groupLabel}:
              <ComboBox
                id="filter-selector_button"
                className="styled-combobox"
                options={[]}
                opened={isOpenSelector}
                selectedOption={selectedOption}
                size="content"
                toggleAction={this.onPeopleSelectorClick}
                displayType="toggle"
                scaled={false}
                noBorder={true}
                directionX="left"
                dropDownMaxHeight={200}
              ></ComboBox>
              <PeopleSelector
                isOpen={isOpenSelector}
                groupsCaption={groupsCaption}
                defaultOption={defaultOption}
                defaultOptionLabel={defaultOptionLabel}
                onCancel={this.onCancelSelector}
                onSelect={this.onSelectGroup}
              />
            </>
          )}
          {!typeSelector && (
            <>
              {groupLabel}:
              {groupItems.length > 1 ? (
                <ComboBox
                  className="styled-combobox"
                  options={groupItems}
                  isDisabled={isDisabled}
                  onSelect={this.onSelect}
                  selectedOption={{
                    key: id,
                    label,
                  }}
                  size="content"
                  scaled={false}
                  noBorder={true}
                  opened={opened}
                  directionX="left"
                  toggleAction={this.toggleCombobox}
                  dropDownMaxHeight={300}
                ></ComboBox>
              ) : (
                <span className="styled-filter-name">{label}</span>
              )}
            </>
          )}
        </StyledFilterItemContent>

        <StyledCloseButtonBlock
          onClick={this.onClick}
          isDisabled={isDisabled}
          isClickable={true}
        >
          <CloseButton isDisabled={isDisabled} onClick={this.onClick} />
        </StyledCloseButtonBlock>
      </StyledFilterItem>
    );
  }
}
FilterItem.propTypes = {
  id: PropTypes.string,
  opened: PropTypes.bool,
  isDisabled: PropTypes.bool,
  block: PropTypes.bool,
  groupItems: PropTypes.array,
  label: PropTypes.string,
  groupLabel: PropTypes.string,
  onClose: PropTypes.func,
  onSelectFilterItem: PropTypes.func,
};

class FilterBlock extends React.Component {
  constructor(props) {
    super(props);

    const { hiddenFilterItems, openFilterItems } = props;

    this.state = {
      hiddenFilterItems: hiddenFilterItems || [],
      openFilterItems: openFilterItems || [],
      needUpdate: false,
    };
  }

  componentDidMount() {
    this.setNeedUpdate(true);
  }

  componentDidUpdate(prevProps, prevState) {
    const { needUpdate } = this.state;
    const { needUpdateFilter } = this.props;
    if (
      (needUpdate || needUpdateFilter) &&
      (!isEqual(prevState.openFilterItems, this.state.openFilterItems) ||
        !isEqual(prevState.hiddenFilterItems, this.state.hiddenFilterItems))
    ) {
      this.props.onFilterRender();
      this.setNeedUpdate(false);
    }
  }

  shouldComponentUpdate(nextProps, nextState) {
    const { hiddenFilterItems, openFilterItems } = nextProps;

    if (!isEqual(this.props, nextProps)) {
      if (
        !isEqual(this.props.hiddenFilterItems, hiddenFilterItems) ||
        !isEqual(this.props.openFilterItems, openFilterItems)
      ) {
        this.setState({
          hiddenFilterItems,
          openFilterItems,
        });
        return false;
      }
      return true;
    }

    return !isEqual(this.state, nextState);
  }

  onDeleteFilterItem = (key) => {
    this.props.onDeleteFilterItem(key);
    this.setNeedUpdate(true);
  };

  setNeedUpdate = (needUpdate) => {
    this.setState({
      needUpdate,
    });
  };
  getFilterItems = () => {
    const { openFilterItems, hiddenFilterItems } = this.state;
    const _this = this;
    let result = [];
    let openItems = [];
    let hideItems = [];
    if (openFilterItems.length > 0) {
      openItems = openFilterItems.map(function (item) {
        const {
          key,
          group,
          groupLabel,
          label,
          typeSelector,
          groupsCaption,
          defaultOptionLabel,
          defaultOption,
          defaultSelectLabel,
          selectedItem,
        } = item;
        return (
          <FilterItem
            block={false}
            isDisabled={_this.props.isDisabled}
            key={key}
            groupItems={_this.props.getFilterData().filter(function (t) {
              return t.group == group && t.group != t.key;
            })}
            onSelectFilterItem={_this.props.onClickFilterItem}
            id={key}
            groupLabel={groupLabel}
            label={label}
            opened={key.indexOf("_-1") == -1 ? false : true}
            onClose={_this.onDeleteFilterItem}
            typeSelector={typeSelector}
            groupsCaption={groupsCaption}
            defaultOptionLabel={defaultOptionLabel}
            defaultOption={defaultOption}
            defaultSelectLabel={defaultSelectLabel}
            selectedItem={selectedItem}
            onFilterRender={_this.props.onFilterRender}
          ></FilterItem>
        );
      });
    }
    if (hiddenFilterItems.length > 0) {
      let open = false;
      let hideFilterItemsList = hiddenFilterItems.map(function (item) {
        const {
          key,
          group,
          groupLabel,
          label,
          typeSelector,
          groupsCaption,
          defaultOptionLabel,
          defaultOption,
          defaultSelectLabel,
          selectedItem,
        } = item;
        open = key.indexOf("_-1") == -1 ? false : true;
        return (
          <FilterItem
            block={true}
            isDisabled={_this.props.isDisabled}
            key={key}
            groupItems={_this.props.getFilterData().filter(function (t) {
              return t.group == group && t.group != t.key;
            })}
            onSelectFilterItem={_this.props.onClickFilterItem}
            id={key}
            groupLabel={groupLabel}
            opened={key.indexOf("_-1") == -1 ? false : true}
            label={label}
            onClose={_this.onDeleteFilterItem}
            typeSelector={typeSelector}
            groupsCaption={groupsCaption}
            defaultOptionLabel={defaultOptionLabel}
            defaultOption={defaultOption}
            defaultSelectLabel={defaultSelectLabel}
            selectedItem={selectedItem}
            onFilterRender={_this.props.onFilterRender}
          ></FilterItem>
        );
      });
      hideItems.push(
        <HideFilter
          key="hide-filter"
          count={hiddenFilterItems.length}
          isDisabled={this.props.isDisabled}
          open={open}
        >
          {hideFilterItemsList}
        </HideFilter>
      );
    }
    result = hideItems.concat(openItems);
    return result;
  };
  getData = () => {
    const _this = this;
    const d = this.props.getFilterData();
    let result = [];
    d.forEach((element) => {
      if (!element.inSubgroup) {
        element.onClick =
          !element.isSeparator && !element.isHeader && !element.disabled
            ? (e) => _this.props.onClickFilterItem(e, element)
            : undefined;
        element.key =
          element.group != element.key
            ? element.group + "_" + element.key
            : element.key;
        if (element.subgroup != undefined) {
          if (d.findIndex((x) => x.group === element.subgroup) == -1)
            element.disabled = true;
        }
        result.push(element);
      }
    });
    return result;
  };

  render() {
    const _this = this;
    const filterItems = this.getFilterItems();
    const filterData = this.props.getFilterData();
    const { iconSize, isDisabled, contextMenuHeader } = this.props;
    return (
      <>
        <div
          className="styled-filter-block"
          ref={this.filterWrapper}
          id="filter-items-container"
        >
          {filterItems}
        </div>
        {filterData.length > 0 && (
          <FilterButton
            columnCount={this.props.columnCount}
            id="filter-button"
            iconSize={iconSize}
            getData={_this.getData}
            isDisabled={isDisabled}
            asideHeader={contextMenuHeader}
          />
        )}
      </>
    );
  }
}
FilterBlock.propTypes = {
  getFilterData: PropTypes.func,
  hiddenFilterItems: PropTypes.array,
  iconSize: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
  isDisabled: PropTypes.bool,
  onDeleteFilterItem: PropTypes.func,
  onRender: PropTypes.func,
  openFilterItems: PropTypes.array,
  columnCount: PropTypes.number,
  contextMenuHeader: PropTypes.string,
};

export default FilterBlock;
