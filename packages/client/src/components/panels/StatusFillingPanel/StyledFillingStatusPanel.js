import styled from "styled-components";
import Scrollbar from "@docspace/components/scrollbar";
import { Base } from "@docspace/components/themes";

const StyledFillingStatusPanel = styled.div`
  .status-filling-panel {
    .scroll-body {
      padding-right: 0 !important;
    }
  }

  .status-filling_header {
    padding: 0 16px;
    border-bottom: ${(props) => props.theme.filesPanels.sharing.borderBottom};

    .status_heading {
      font-weight: 700;
      font-size: 21px;
      margin: 12px 0;
    }
  }

  .status-filling_sub-header {
    padding: 16px 16px 12px;
    font-weight: 600;
    font-size: 14px;
    line-height: 16px;
    color: rgba(255, 255, 255, 0.6);
  }

  .status-filling_item {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 9px 16px;
    background: #282828;
    width: 94%;
    height: 50px;
    border-radius: 6px;
    margin: 0 auto;

    .item-title {
      display: flex;
      align-items: center;
      justify-content: space-between;

      .icon {
        width: 24px;
        height: 24px;
        margin-right: 15px;
        svg {
          width: 24px;
          height: 24px;
        }
      }
    }
  }

  .status-filling_footer {
    .footer-text {
      margin-bottom: 16px;
      color: rgba(255, 255, 255, 0.6);
    }

    display: block;
    position: fixed;
    bottom: 0;
    left: 0;
    right: 0;
    padding: 16px;
    width: 100%;
    background-color: ${(props) =>
      props.theme.filesPanels.footer.backgroundColor};
    border-top: ${(props) => props.theme.filesPanels.footer.borderTop};
    box-sizing: border-box;
  }
`;

const StyledScrollbar = styled(Scrollbar)`
  position: relative;
  padding: 16px 0;
  height: calc(100vh - 87px) !important;
`;

StyledFillingStatusPanel.defaultProps = { theme: Base };

export { StyledFillingStatusPanel, StyledScrollbar };
