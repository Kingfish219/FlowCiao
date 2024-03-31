import { useCallback, useState, useContext, useEffect } from "react";
import { Handle, Position, useStore } from "reactflow";
import { Button, Dropdown, Space, Modal } from "antd";
import exitActionImg from "../Assets/exit-action.svg";
import entryActionImg from "../Assets/entry-action.svg";
import dotImg from "../Assets/dot.svg";
import plusImg from "../Assets/circle-plus.svg";
import actionIconImg from "../Assets/action-icon.svg";
import trashImg from "../Assets/trash.svg";
import ApplicationContext from "../Store/ApplicationContext";
import NodeActvityModal from "./NodeActvityModal";

const IdleNode = (node) => {
  const appCtx = useContext(ApplicationContext);
  console.log(appCtx)
  const onChange = useCallback((evt) => {
    console.log(evt.target.value);
  }, []);

  const [isHoverNode, setIsHoverNode] = useState(false);
  const [isHoverSpaceNode, setIsHoverSpaceNode] = useState(false);

  const onIdleNodeHoverFunc = () => {
    setIsHoverNode(true);
  };

  const onIdleNodeLoseHoverFunc = (event) => {
    setIsHoverNode(false);
  };

  const onIdleNodeHoverSpaceFunc = () => {
    setIsHoverSpaceNode(true);
  };

  const onIdleNodeLoseHoverSpaceFunc = (event) => {
    setIsHoverSpaceNode(false);
  };

  const onAddIdleNodeClick = () => {
    node.data.AddIdleNodeFunc(node);
  };

  const onNodeNameChange = (e) => {
    node.data.Name = e.target.value;
  };
  const [nodeActivities, setNodeActivities] = useState([]);

  useEffect(() => {
    let updatedActivities = [];
    if (node.data.onEntry != "") {
      updatedActivities.push(node.data.onEntry);
    }
    if (node.data.onExit != "") {
      updatedActivities.push(node.data.onExit);
    }
    setNodeActivities(updatedActivities);
  },[])

  const [isModalOpen, setIsModalOpen] = useState(false);
  const showModal = () => {
    setIsModalOpen(true);
  };
 const onApplyChanges = (activities) => {
    if (activities != null) {
      node.data.onEntry = activities.onEntryName;
      node.data.onExit = activities.onExitName;
      let updatedActivities = [];
      if (node.data.onEntry != "") {
        updatedActivities.push(node.data.onEntry);
      }
      if (node.data.onExit != "") {
        updatedActivities.push(node.data.onExit);
      }
      setNodeActivities(updatedActivities);
    }
    setIsModalOpen(false);
  };

  return (
    <>
      <div
        onMouseEnter={onIdleNodeHoverFunc}
        onMouseLeave={onIdleNodeLoseHoverFunc}
        ref={(el) => {
          if (el) {
            el.style.setProperty(
              "border-top-color",
              appCtx.Theme.borderColor,
              "important"
            );
          }
        }}
      >
        <Handle
          type="target"
          isConnectable={true}
          position={Position.Left}
          className="node-handle"
        />
        <Handle
          type="target"
          isConnectable={true}
          position={Position.Right}
          className="node-handle"
        />
        <Handle
          type="target"
          isConnectable={true}
          position={Position.Top}
          className="node-handle"
        />
        <Handle
          type="target"
          isConnectable={true}
          position={Position.Bottom}
          className="node-handle"
        />
        <Handle
          type="source"
          isConnectable={true}
          className="node-handle"
          position={Position.Right}
        />
        <Handle
          type="source"
          isConnectable={true}
          className="node-handle"
          position={Position.Left}
        />
        <Handle
          type="source"
          isConnectable={true}
          className="node-handle"
          position={Position.Top}
        />
        <Handle
          type="source"
          isConnectable={true}
          className="node-handle"
          position={Position.Bottom}
        />
        <button
          id="addIdleNode"
          className="add-node-btn"
          onClick={onAddIdleNodeClick}
        >
          {isHoverNode ? <img src={plusImg} /> : <img src={dotImg} />}
        </button>
        {!isHoverNode && !isHoverSpaceNode ? (
          <></>
        ) : (
          <button className={nodeActivities.length > 0 ? "node-actvity-btn" : "node-actvity-btn no-activity"}  onClick={showModal}>
            {nodeActivities.length > 0 ? (
              <span className="node-activity-count-container">
                <img className="active-filter" src={actionIconImg} />
                <span>{nodeActivities.length}</span>
              </span>
            ) : (
              <span className="node-activity-count-container no-activity">
                <img src={actionIconImg} />
              </span>
            )}
          </button>
        )}
        <div
          className="node-actvity-btn-hover-space"
          onMouseEnter={onIdleNodeHoverSpaceFunc}
          onMouseLeave={onIdleNodeLoseHoverSpaceFunc}
        ></div>
        <input
          className="node-name"
          type="text"
          placeholder="Pending"
          defaultValue={node.data.Name != "" ? node.data.Name : "Pending"}
          onChange={onNodeNameChange}
        />
      </div>
      <NodeActvityModal
        node={node}
        onApplyChanges={onApplyChanges}
        isModalOpen={isModalOpen}
      />
    </>
  );
};
export default IdleNode;
