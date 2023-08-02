import React from "react";
import { inject, observer } from "mobx-react";

import ModalDialog from "@docspace/components/modal-dialog";

import WrappedComponent from "SRC_DIR/helpers/plugins/WrappedComponent";
import { PluginComponents } from "SRC_DIR/helpers/plugins/constants";
import { messageActions } from "SRC_DIR/helpers/plugins/utils";

const PluginDialog = ({
  isVisible,
  dialogHeader,
  dialogBody,
  dialogFooter,
  onClose,
  onLoad,
  eventListeners,

  pluginId,
  setSettingsPluginDialogVisible,
  setCurrentSettingsDialogPlugin,
  updatePluginStatus,

  setPluginDialogVisible,
  setPluginDialogProps,
  ...rest
}) => {
  const [dialogHeaderProps, setDialogHeaderProps] =
    React.useState(dialogHeader);
  const [dialogBodyProps, setDialogBodyProps] = React.useState(dialogBody);
  const [dialogFooterProps, setDialogFooterProps] =
    React.useState(dialogFooter);

  const functionsRef = React.useRef([]);

  const onCloseAction = async () => {
    const message = await onClose();

    messageActions(
      message,
      null,
      null,
      pluginId,
      setSettingsPluginDialogVisible,
      setCurrentSettingsDialogPlugin,
      updatePluginStatus,
      null,
      setPluginDialogVisible,
      setPluginDialogProps
    );
  };

  React.useEffect(() => {
    if (eventListeners) {
      eventListeners.forEach((e) => {
        const onAction = () => {
          const message = e.onAction();

          messageActions(
            message,
            null,
            null,
            pluginId,
            setSettingsPluginDialogVisible,
            setCurrentSettingsDialogPlugin,
            updatePluginStatus,
            null,
            setPluginDialogVisible,
            setPluginDialogProps
          );
        };

        functionsRef.push(onAction);

        document.addEventListener(e.name, onAction);
      });
    }

    return () => {
      if (eventListeners) {
        eventListeners.forEach((e, index) => {
          document.removeEventListener(e.name, functionsRef[index]);
        });
      }
    };
  }, []);

  const onLoadAction = React.useCallback(async () => {
    if (onLoad) {
      const res = await onLoad();

      setDialogHeaderProps(res.newDialogHeader);
      setDialogBodyProps(res.newDialogBody);
      setDialogFooterProps(res.newDialogFooter);
    }
  }, [onLoad]);

  React.useEffect(() => {
    onLoadAction();
  }, [onLoadAction]);

  return (
    <ModalDialog visible={isVisible} onClose={onCloseAction} {...rest}>
      <ModalDialog.Header>{dialogHeaderProps}</ModalDialog.Header>
      <ModalDialog.Body>
        <WrappedComponent
          pluginId={pluginId}
          component={{
            component: PluginComponents.box,
            props: dialogBodyProps,
          }}
        />
      </ModalDialog.Body>
      {dialogFooter && (
        <ModalDialog.Footer>
          <WrappedComponent
            pluginId={pluginId}
            component={{
              component: PluginComponents.box,
              props: dialogFooterProps,
            }}
          />
        </ModalDialog.Footer>
      )}
    </ModalDialog>
  );
};

export default inject(({ pluginStore }) => {
  const {
    pluginDialogProps,
    setSettingsPluginDialogVisible,
    setCurrentSettingsDialogPlugin,
    updatePluginStatus,

    setPluginDialogVisible,
    setPluginDialogProps,
  } = pluginStore;

  return {
    ...pluginDialogProps,
    setSettingsPluginDialogVisible,
    setCurrentSettingsDialogPlugin,
    updatePluginStatus,

    setPluginDialogVisible,
    setPluginDialogProps,
  };
})(observer(PluginDialog));
