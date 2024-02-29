import React, {
  useState,
  useRef,
  useCallback,
  forwardRef,
  useImperativeHandle,
  useEffect,
} from "react";
import ReactFlow, {
  ReactFlowProvider,
  addEdge,
  useNodes,
  useEdges,
  updateEdge,
  useNodesState,
  useEdgesState,
  useStoreState,
  applyEdgeChanges,
  applyNodeChanges,
  useReactFlow,
  Controls,
} from "reactflow";
import "reactflow/dist/style.css";
import { DeleteOutlined } from "@ant-design/icons";
import IdleNode from "./IdleNode";
import StartNode from "./StartNode";

import Sidebar from "./Sidebar";

import "./Flow.css";
import CustomEdge from "./CustomEdge";

import CustomConnectionLine from "./CustomConnectionLine";

const nodeTypes = { idleNode: IdleNode, StartNode: StartNode };

const edgeTypes = { "custom-edge": CustomEdge };

let id = 2;
const getId = () => `${id++}`;

const Flow = forwardRef((props, ref) => {
  const edgeUpdateSuccessful = useRef(true);
  const reactFlowWrapper = useRef(null);
  const [reactFlowInstance, setReactFlowInstance] = useState(null);
  const nodesPosition = useRef([{ x: 0, y: 0 }]);

  const findFirstEmptyPosition = (xCoordinate, yCoordinate) => {
    const positionsAtX = nodesPosition.current.filter(
      (position) => position.x === xCoordinate
    );
    let expectedY = yCoordinate;
    for (let i = 0; i < positionsAtX.length; i++) {
      if (positionsAtX.find((pos) => pos.y === expectedY) === undefined) {
        return { x: xCoordinate, y: expectedY };
      }
      expectedY += 150;
    }
    return { x: xCoordinate, y: expectedY };
  };

  const onAddIdleNodeFunc = (currentNode) => {
    var newNodeXPos = currentNode.xPos + 250;
    const position = findFirstEmptyPosition(newNodeXPos, currentNode.yPos);
    nodesPosition.current.push(position);
    const id = getId();
    const newNode = {
      id,
      position: position,
      type: "idleNode",
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
        Name: "",
        onEntry: "",
        onExit: ""
      },
      origin: [0.5, 0.0],
    };
    setNodes((nds) => nds.concat(newNode));
    setEdges((eds) =>
      eds.concat({
        id,
        source: currentNode.id,
        target: id,
        type: "custom-edge",
        data:{
            Name: ""
        }
      })
    );
  };
  const onNodesDelete = (node) => {
    if (node[0].id === "1" || node[0].id === "2") {
      return;
    }
    const index = nodesPosition.current.indexOf(node[0].position);
    if (index > -1) {
      nodesPosition.current.splice(index, 1);
    }
  };

  const initialEdges = [
    { id: "1", source: "0", target: "1", type: "custom-edge" , data:{Name: "start"}},
  ];
  const [edges, setEdges] = useEdgesState(initialEdges);

  const initialNodes = [
    {
      id: "0",
      type: "StartNode",
      position: { x: 0, y: 0 },
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
      },
    },
    {
      id: "1",
      type: "idleNode",
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
        Name: "",
        onEntry: "",
        onExit: ""
      },
      origin: [0.5, 0.0],
      position: { x: 200, y: 0 },
    },
  ];
  const [nodes, setNodes] = useNodesState(initialNodes);

  useImperativeHandle(ref, () => ({
    exportFlowAsJSON,
  }));

  const exportFlowAsJSON = () => {
    const flowData = {
      elements: [...nodes, ...edges],
    };
    const jsonFlow = JSON.stringify(flowData, null);
    console.log("Exported JSON:", jsonFlow);
  };
  const onNodesChange = useCallback(
    (changes) => {
      if (
        changes[0].type === "remove" &&
        (changes[0].id === "1" || changes[0].id === "2")
      ) {
        return;
      }
      setNodes((nds) => applyNodeChanges(changes, nds));
    },
    [setNodes]
  );

  const onEdgesChange = useCallback(
    (changes) => {
      if (changes[0].type === "remove" && changes[0].id === "2") {
        return;
      }
      setEdges((eds) => applyEdgeChanges(changes, eds));
    },
    [setEdges]
  );

  const onEdgeUpdateStart = useCallback(() => {
    edgeUpdateSuccessful.current = false;
  }, []);

  const onEdgeUpdate = useCallback((oldEdge, newConnection) => {
    edgeUpdateSuccessful.current = true;
    setEdges((els) => updateEdge(oldEdge, newConnection, els));
  }, []);

  const onEdgeUpdateEnd = useCallback((_, edge) => {
    if (!edgeUpdateSuccessful.current) {
      setEdges((eds) => eds.filter((e) => e.id !== edge.id));
    }

    edgeUpdateSuccessful.current = true;
  }, []);

  const onConnect = useCallback(
    (connection) => {
      const edge ={id: getId(), source:connection.source,target:connection.target,type:"custom-edge",data:{Name:""}} //{ ...connection, type: "custom-edge" };

      setEdges((eds) => addEdge(edge, eds));
    },
    [setEdges]
  );

  const onDeleteNode = (nodeId) => {
    const updatedNodes = nodes.filter((node) => node.id !== nodeId);
    setNodes(updatedNodes);
  };

  const onDragOver = useCallback((event) => {
    event.preventDefault();
    event.dataTransfer.dropEffect = "move";
  }, []);

  const onDrop = useCallback(
    (event) => {
      event.preventDefault();
      console.log(event);
      const reactFlowBounds = reactFlowWrapper.current.getBoundingClientRect();
      const type = event.dataTransfer.getData("application/reactflow");

      // check if the dropped element is valid
      if (typeof type === "undefined" || !type) {
        return;
      }

      const position = reactFlowInstance.project({
        x: event.clientX - reactFlowBounds.left,
        y: event.clientY - reactFlowBounds.top,
      });

      const nodeStyle =
        type == "default" || type == "idleNode"
          ? {
              width: "127px",
              height: "49px",
              border: "1px solid #1a192b",
              borderRadius: "9px",
              marginBottom: "10px",
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              cursor: "grab",
            }
          : {
              width: "110px",
              minHeight: "48px",
              border: "1px solid #C8C8C8",
              borderTop: "3px solid #2F6EE9",
              borderRadius: 4,
              display: "flex",
              justifyContent: "center",
              alignItems: "center",
              cursor: "grab",
            };
      const newNode = {
        id: getId(),
        type,
        position,
        sourcePosition: "right",
        targetPosition: "left",
        data: { label: `${type == "default" ? "Idle" : "End"}` },
        style: nodeStyle,
      };

      setNodes((nds) => nds.concat(newNode));
    },

    [reactFlowInstance]
  );

  useEffect(() => {
    if (props.resetFlowCalled) {
      setNodes(initialNodes);
      setEdges(initialEdges);
      nodesPosition.current = [{ x: 0, y: 0 }];
      id = 2;
      props.onResetFlowClick(false);
    }
  }, [props.resetFlowCalled]);

  const connectionLineStyle = {
    strokeWidth: 1.5,
    stroke: "#b1b1b7",
  };

  return (
    <div className="dndflow">
      <ReactFlowProvider>
        {/* <Sidebar /> */}
        <div className="reactflow-wrapper" ref={reactFlowWrapper}>
          <ReactFlow
            nodes={nodes}
            edges={edges}
            onNodesChange={onNodesChange}
            onEdgesChange={onEdgesChange}
            // onNodeClick={onNodesClick}
            deleteKeyCode={["Backspace", "Delete"]}
            onNodesDelete={onNodesDelete} // as needed
            // onEdgesDelete={}   // as needed
            onConnect={onConnect}
            onEdgeUpdate={onEdgeUpdate}
            onEdgeUpdateStart={onEdgeUpdateStart}
            onEdgeUpdateEnd={onEdgeUpdateEnd}
            onInit={setReactFlowInstance}
            nodeTypes={nodeTypes}
            edgeTypes={edgeTypes}
            // onDrop={onDrop}
            // onDragOver={onDragOver}
            connectionLineComponent={CustomConnectionLine}
            connectionLineStyle={connectionLineStyle}
            fitView
          >
            <Controls />
          </ReactFlow>
        </div>
      </ReactFlowProvider>
    </div>
  );
});

export default Flow;
