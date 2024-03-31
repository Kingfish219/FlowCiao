import React from "react";
import {
  getBezierPath,
  EdgeLabelRenderer,
  BaseEdge,
} from "reactflow";

const CustomEdge = ({
  id,
  sourceX,
  sourceY,
  targetX,
  targetY,
  sourcePosition,
  targetPosition,
  data,
}) => {
  const [edgePath, labelX, labelY] = getBezierPath({
    sourceX,
    sourceY,
    sourcePosition,
    targetX,
    targetY,
    targetPosition,
  });

  const onEdgeNameChange = (e) => {
    data.Name = e.target.value;
  };

  return (
    <>
      <BaseEdge
        id={id}
        path={edgePath}
        style={{ stroke: "#393939" }}
        markerEnd="url(#arrowclosed)"
      />
      <EdgeLabelRenderer>
        <div
          className="custom-edge-container"
          style={{
            transform: `translate(-50%, -50%) translate(${labelX}px,${labelY}px)`,
          }}
        >
          <input
            className="custom-edge-input"
            placeholder="some action"
            defaultValue={data.Name != "" ? data.Name : "some action"}
            onChange={onEdgeNameChange}
          />
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
