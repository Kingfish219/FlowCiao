import React, { useCallback } from "react";
import {
  useStore,
  getBezierPath,
  EdgeLabelRenderer,
  BaseEdge,
  useEdges,
  useNodes
} from "reactflow";
import { Input } from "antd";
const { TextArea } = Input;

const CustomEdge = ({
  id,
  source,
  target,
  sourceX,
  sourceY,
  targetX,
  targetY,
  sourcePosition,
  targetPosition,
  data,
}) => {
  const edges = useEdges();
const nodes = useNodes();

  let path = "",
    edgeLabelX = "",
    edgeLabelY = "";
  const [edgePath, labelX, labelY] = getBezierPath({
    sourceX,
    sourceY,
    sourcePosition,
    targetX,
    targetY,
    targetPosition,
  });

  path = edgePath;
  edgeLabelX = labelX;
  edgeLabelY = labelY;

  if (source === target) {
    const radiusX = (sourceX - targetX) * 0.6;
    const radiusY = 50;
    path = `M ${sourceX - 5} ${sourceY} A ${radiusX} ${radiusY} 0 1 0 ${
      targetX + 2
    } ${targetY}`;
    edgeLabelY = labelY - radiusX + 17;
  } else if (
    edges.some(
      (e) =>
        (e.source === target && e.target === source) ||
        (e.target === source && e.source === target)
    )
  ) {
    const centerX = (sourceX + targetX) / 2;
    const centerY = (sourceY + targetY) / 2;
    const offset = sourceX < targetX ? 50 : (sourceX - targetX) * -0.2;

    if (targetX < sourceX) {
      edgeLabelY = labelY - 50;

      path = `M ${sourceX} ${sourceY} Q ${centerX} ${
        centerY + offset
      } ${targetX} ${targetY}`;

    } else if (Math.abs(sourceY - targetY) > 20) {

      path = `M ${sourceX} ${sourceY} Q ${centerX} ${
        centerY + offset
      } ${targetX} ${targetY}`;
      
      edgeLabelY = labelY - 17 + (sourceX - targetX) * -0.2;
    }
  }

  const onEdgeNameChange = (e) => {
    data.Name = e.target.value;
  };

  return (
    <>
      <BaseEdge
        id={id}
        path={path}
        style={{ stroke: "#393939" }}
        markerEnd="url(#arrowclosed)"
      />
      <EdgeLabelRenderer>
        <div
          className="custom-edge-container"
          style={{
            transform: `translate(-50%, -50%) translate(${edgeLabelX}px,${edgeLabelY}px)`,
          }}
        >
          {data.noInput == undefined || !data.noInput ? (
            <TextArea
              className="custom-edge-input"
              placeholder="New Trigger"
              defaultValue={data.Name != "" ? data.Name : "New Trigger"}
              onChange={onEdgeNameChange}
              autoSize={{
                minRows: 1,
                maxRows: 6,
              }}
            />
          ) : (
            <></>
          )}
        </div>
      </EdgeLabelRenderer>

      <defs>
        <marker
          id="arrowclosed"
          markerWidth="8"
          markerHeight="8"
          refX="5"
          refY="4"
          orient="auto"
          markerUnits="strokeWidth"
        >
          <path d="M1,1 L1,7 L5,4 L1,1" fill="#393939" />
        </marker>
      </defs>
    </>
  );
};

export default CustomEdge;
