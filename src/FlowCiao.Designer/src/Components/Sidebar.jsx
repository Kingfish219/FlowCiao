import React from "react";
import IdleNode from "./IdleNode";

export default () => {
  const onDragStart = (event, nodeType) => {
    event.dataTransfer.setData("application/reactflow", nodeType);
    event.dataTransfer.effectAllowed = "move";
  };

  return (
    <aside>
      <div
        className="dndnode input circle-node"
        onDragStart={(event) => onDragStart(event, "output")}
        draggable
      >
        End
      </div>

      <div
        className="dndnode"
        onDragStart={(event) => onDragStart(event, "idleNode")}
        draggable
      >
        Idle
      </div>

      <div className="dndnode circle-node disabled" disabled>
        Activity
      </div>
      <div className="dndnode circle-node disabled" disabled>
        Condition
      </div>
    </aside>
  );
};
