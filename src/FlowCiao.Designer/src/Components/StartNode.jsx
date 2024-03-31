import { useCallback, useState } from "react";
import { Handle, Position } from "reactflow";
import { Button, Dropdown, Space } from "antd";
import dotImg from "../Assets/dot.svg"
import plusImg from "../Assets/circle-plus.svg"

const StartNode = (node) => {

  const [isHoverNode, setIsHoverNpde] = useState(false);

  const onNodeHoverFunc = () => {
    setIsHoverNpde(true);
  };

  const onNodeLoseHoverFunc = () => {
    setIsHoverNpde(false);
  };

  const onAddIdleNodeClick = () => {
    node.data.AddIdleNodeFunc(node);
  }

  return (
    <div className="start-node"
    //  onMouseEnter={onNodeHoverFunc}
    // onMouseLeave={onNodeLoseHoverFunc}
  >
    {/* <button
      id="addIdleNode"
      className="add-node-btn"
      onClick={onAddIdleNodeClick}
    >
      {isHoverNode ? (
        <img src={plusImg}/>
      ) : (
        <img src={dotImg}/>
      )}
    </button> */}
     <div className="add-node-btn"><img src={dotImg}/></div>
      <Handle
        type="source"
        isConnectable={false}
        className="node-handle"
        position={Position.Right}
        id="s1"
      />
    </div>
  );
};
export default StartNode;
