import React from "react";
import { getBezierPath, EdgeLabelRenderer, BaseEdge } from "reactflow";

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

  return (
    <>
      <BaseEdge id={id} path={edgePath} />
      <EdgeLabelRenderer>
        <div
          className="custom-edge-container"
          style={{
            transform: `translate(-50%, -50%) translate(${labelX}px,${labelY}px)`,
          }}
        >
          <input className="custom-edge-input" placeholder="some action" defaultValue={"some action"}/>
        </div>
      </EdgeLabelRenderer>
    </>
  );
};

export default CustomEdge;
