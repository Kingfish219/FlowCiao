import { useCallback, useState, useContext } from "react";
import { Handle, Position, useStore} from "reactflow";
import { Button, Dropdown, Space } from "antd";
import exitActionImg from "../Assets/exit-action.svg";
import entryActionImg from "../Assets/entry-action.svg";
import dotImg from "../Assets/dot.svg";
import plusImg from "../Assets/circle-plus.svg";
import threeDotImg from "../Assets/circle-threedot.svg";
import trashImg from "../Assets/trash.svg";
import ThemeContext from "../Store/ThemeContext";

const IdleNode = (node) => {

  const themeCtx = useContext(ThemeContext);
  const onChange = useCallback((evt) => {
    console.log(evt.target.value);
  }, []);

  const [isHoverNode, setIsHoverNpde] = useState(false);

  const onIdleNodeHoverFunc = () => {
    setIsHoverNpde(true);
  };

  const onIdleNodeLoseHoverFunc = () => {
    setIsHoverNpde(false);
  };

  const onAddIdleNodeClick = () => {
    node.data.AddIdleNodeFunc(node);
  };

  const [isEntryActionSelected, setIsEntryActionSelected] = useState(false);
  const [isExitActionSelected, setIsExitActionSelected] = useState(false);
  const items = [
    {
      key: "entryAction",
      label: (
        <span>
          <img src={entryActionImg} />
          <span className="node-action-btn">
            On Entry
          </span>
        </span>
      ),
      disabled: isEntryActionSelected,
    },
    {
      type: "divider",
    },
    {
      key: "exitAction",
      label: (
        <span>
          <img src={exitActionImg} />
          <span className="node-action-btn">
            On Exit
          </span>
        </span>
      ),
      disabled: isExitActionSelected,
    },
  ];

  const chooseActionHandler = ( {key}) => {
    if (key == "entryAction") {
      setIsEntryActionSelected(true);
    } else if (key == "exitAction") {
      setIsExitActionSelected(true);
    }
  };
  const removeEntryActionHandler = () => {
    setIsEntryActionSelected(false);
  };
  const removeExitActionHandler = () => {
    setIsExitActionSelected(false);
  };
  return (
    <div
      onMouseEnter={onIdleNodeHoverFunc}
      onMouseLeave={onIdleNodeLoseHoverFunc}
      ref={el => {
        if (el) {
          el.style.setProperty('border-top-color', themeCtx.borderColor, 'important');
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
      <Dropdown
        menu={{
          items,
          onClick: chooseActionHandler ,
        }}
        placement="bottomRight"
      >
        <button
        className="add-node-function-btn"
        >
          <img src={threeDotImg} />
        </button>
      </Dropdown>
      <input
      className="node-name"
        type="text"
        placeholder="Pending"
         defaultValue={"Pending"}
      />
      {(isEntryActionSelected || isExitActionSelected) && (
        <div className="func-container">
          <span className="func-container-title">
            Actvities
          </span>
          {isEntryActionSelected && (
            <div className="node-func">
              <img
                src={entryActionImg}
                className="image-svg-2F6EE9-color"
              />
              <input className="action-func-name"
                placeholder="Custom Act"
                defaultValue={"Custom Act"}
              />
              <button
                className="func-action-remove-btn"
                onClick={removeEntryActionHandler}
              >
                <img src={trashImg} width={8} />
              </button>
            </div>
          )}

          {isExitActionSelected && (
            <div className="node-func">
              <img
                src={exitActionImg}
                className="image-svg-2F6EE9-color"
              />
              <input
                className="action-func-name"
                placeholder="Custom Act"
                defaultValue={"Custom Act"}
              />
              <button
                className="func-action-remove-btn"
                onClick={removeExitActionHandler}
              >
                <img src={trashImg} width={8} />
              </button>
            </div>
          )}
        </div>
      )}
    </div>
  );
};
export default IdleNode;
