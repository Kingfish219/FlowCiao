import React from "react";
import { getBezierPath, EdgeLabelRenderer, BaseEdge } from "reactflow";
import { Input } from 'antd';
const { TextArea } = Input;

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
          {data.noInput == undefined || !data.noInput ? (
            // <input
            //   className="custom-edge-input"
            //   placeholder="New Trigger"
            //   defaultValue={data.Name != "" ? data.Name : "New Trigger"}
            //   onChange={onEdgeNameChange}
            // />
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
