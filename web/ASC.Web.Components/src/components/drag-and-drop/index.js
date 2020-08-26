import React from "react";
import { useDropzone } from "react-dropzone";
import PropTypes from "prop-types";
import styled from "styled-components";

const StyledDragAndDrop = styled.div`
  /*-webkit-touch-callout: none;
  -webkit-user-select: none;
  -moz-user-select: none;
  -ms-user-select: none;
  user-select: none;*/
  height: 100%;
  border: ${props => props.drag ? "1px dashed #bbb" : "1px solid transparent"};
  outline: none;
  background: ${props => props.dragging ? props.isDragAccept ? "#EFEFB2" : "#F8F7BF" : "none"};
`;

const DragAndDrop = props => {
  const { isDropZone, children, dragging, className, ...rest } = props;
  const classNameProp = className ? className : "";

  const onDrop = (acceptedFiles, array, e) => {
    acceptedFiles.length && props.onDrop && props.onDrop(acceptedFiles, e);
  };

  const { getRootProps, isDragActive } = useDropzone({
    noDragEventsBubbling: !isDropZone,
    onDrop
  });

  return (
    <StyledDragAndDrop
      {...rest}
      className={`drag-and-drop draggable${classNameProp}`}
      dragging={dragging}
      isDragAccept={isDragActive}
      drag={isDragActive && isDropZone}
      {...getRootProps()}
    >
      {children}
    </StyledDragAndDrop>
  );
};

DragAndDrop.propTypes = {
  children: PropTypes.any,
  className: PropTypes.string,
  isDropZone: PropTypes.bool,
  dragging: PropTypes.bool,
  onMouseDown: PropTypes.func,
  onDrop: PropTypes.func
};

export default DragAndDrop;
