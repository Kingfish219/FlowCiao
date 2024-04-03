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
import IdleNode from "./IdleNode";
import StartNode from "./StartNode";

import "./Flow.css";
import CustomEdge from "./CustomEdge";

import CustomConnectionLine from "./CustomConnectionLine";

const nodeTypes = { idleNode: IdleNode, StartNode: StartNode };

const edgeTypes = { "custom-edge": CustomEdge };

localStorage.setItem("lastNodeId", 2);
const getId = () => {
  let id = localStorage.getItem("lastNodeId");
  id++;
  localStorage.setItem("lastNodeId", id);
  return `${id}`;
};
const setId = (id) => {
  localStorage.setItem("lastNodeId", id == undefined ? 2 : id);
};

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
    var newNodeXPos = currentNode.xPos + 350;
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
        onExit: "",
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
        data: {
          Name: "",
        },
      })
    );
  };
  const onNodesDelete = (node) => {
    var startNode = nodes.find((x) => x.type === "StartNode");
    var firstIdleNodeId = edges.find((x) => x.source === startNode.id).target;
    if (node[0].id === startNode.id || node[0].id === firstIdleNodeId) {
      return;
    }
    const index = nodesPosition.current.indexOf(node[0].position);
    if (index > -1) {
      nodesPosition.current.splice(index, 1);
    }
  };

  const initialEdges = [
    {
      id: "2",
      source: "1",
      target: "2",
      type: "custom-edge",
      data: { Name: "", noInput: true },
    },
  ];
  const [edges, setEdges] = useEdgesState(initialEdges);

  const initialNodes = [
    {
      id: "1",
      type: "StartNode",
      position: { x: 0, y: 15 },
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
      },
    },
    {
      id: "2",
      type: "idleNode",
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
        Name: "",
        onEntry: "",
        onExit: "",
      },
      origin: [0.5, 0.0],
      position: { x: 200, y: 0 },
    },
  ];
  const [nodes, setNodes] = useNodesState(initialNodes);

  useImperativeHandle(ref, () => ({
    exportFlowAsJSON,
    importJson,
  }));

  const exportFlowAsJSON = () => {
    const flowData = {
      nodes: [...nodes],
      trigers: [...edges],
    };
    var startNode = nodes.find((x) => x.type === "StartNode");
    var firstIdleNodeId = edges.find((x) => x.source === startNode.id).target;

    var states = nodes
      .filter((x) => x.id !== startNode.id)
      .map((node) => ({ Code: node.id, Name: node.data.Name }));

    var actions = edges
      .filter((x) => x.id !== firstIdleNodeId)
      .map((edge) => ({ Code: edge.id, Name: edge.data.Name }));

    var initial = {
      fromStateCode: firstIdleNodeId,
      allows: edges
        .filter((edge) => edge.source === firstIdleNodeId)
        .map((item) => ({
          allowedStateCode: item.target,
          actionCode: item.id,
        })),
      onEntry:
        nodes.find((x) => x.id === firstIdleNodeId).data.onEntry != ""
          ? { name: nodes.find((x) => x.id === firstIdleNodeId).data.onEntry }
          : "",
      onExit:
        nodes.find((x) => x.id === firstIdleNodeId).data.onExit != ""
          ? { name: nodes.find((x) => x.id === firstIdleNodeId).data.onExit }
          : "",
    };

    var steps = edges
      .filter((x) => x.source !== startNode.id && x.source !== firstIdleNodeId)
      .map((item) => ({
        fromStateCode: item.source,
        allows: edges
          .filter((edge) => edge.source === item.source)
          .map((item2) => ({
            allowedStateCode: item2.target,
            actionCode: item2.id,
          })),
        onEntry:
          nodes.find((x) => x.id === item.source).data.onEntry != ""
            ? { name: nodes.find((x) => x.id === item.source).data.onEntry }
            : "",
        onExit:
          nodes.find((x) => x.id === item.source).data.onExit != ""
            ? { name: nodes.find((x) => x.id === item.source).data.onExit }
            : "",
      }));

    const removeDuplicates = (arr) => {
      const uniqueMap = new Map();
      arr.forEach((item) => uniqueMap.set(item.fromStateCode, item));
      return Array.from(uniqueMap.values());
    };

    // Apply removeDuplicates function to the mapped array
    steps = removeDuplicates(steps);

    var endSteps = states
      .filter(
        (x) =>
          steps.concat(initial).find((y) => y.fromStateCode == x.Code) ==
          undefined
      )
      .map((item) => ({
        fromStateCode: item.Code,
        onEntry:
          nodes.find((x) => x.id === item.Code).data.onEntry != ""
            ? { name: nodes.find((x) => x.id === item.Code).data.onEntry }
            : "",
        onExit:
          nodes.find((x) => x.id === item.Code).data.onExit != ""
            ? { name: nodes.find((x) => x.id === item.Code).data.onExit }
            : "",
      }));

    const flow = {
      Key: props.workflowName,
      Name: props.workflowName,
      States: states,
      Actions: actions,
      Initial: initial,
      Steps: steps.concat(endSteps),
    };

    const jsonFlow = JSON.stringify(flowData, null);
    downloadJSON(JSON.stringify(flow, null));
    // console.log("Exported JSON:", jsonFlow);
    // console.log("Exported JSON22: ", JSON.stringify(flow, null));
  };

  const downloadJSON = (jsonString) => {
    const currentDateTime = new Date();
    const dateTime =
      currentDateTime.getFullYear().toString() +
      currentDateTime.getMonth().toString() +
      currentDateTime.getDay().toString() +
      "_" +
      currentDateTime.getHours().toString() +
      currentDateTime.getMinutes().toString() +
      currentDateTime.getSeconds().toString();
    const blob = new Blob([jsonString], { type: "application/json" });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.style.display = "none";
    a.href = url;
    a.download = props.workflowName + `_jsonFlow_${dateTime}.json`; // Set the filename here
    document.body.appendChild(a);
    a.click();
    URL.revokeObjectURL(url);
    document.body.removeChild(a);
  };

  const importJson = (jsonFlow) => {
    nodesPosition.current = [{ x: 0, y: 0 }];
    setId();

    var importedNodes = jsonFlow.States.map((node) => ({
      id: node.Code,
      type: "idleNode",
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
        Name: node.Name,
        onEntry: "",
        onExit: "",
      },
      origin: [0.5, 0.0],
      position: null,
    }));

    var importedEdges = jsonFlow.Actions.map((edge) => ({
      id: edge.Code,
      source: "",
      target: "",
      type: "custom-edge",
      data: { Name: edge.Name },
    }));

    var firstIdleNode = importedNodes.find(
      (x) => x.id == jsonFlow.Initial.fromStateCode
    );
    firstIdleNode.data.reset = true;
    firstIdleNode.data.onEntry =
      jsonFlow.Initial.onEntry != undefined && jsonFlow.Initial.onEntry != ""
        ? jsonFlow.Initial.onEntry.name
        : "";
    firstIdleNode.data.onExit =
      jsonFlow.Initial.onExit != undefined && jsonFlow.Initial.onExit != ""
        ? jsonFlow.Initial.onExit.name
        : "";
    firstIdleNode.position = { x: 200, y: 0 };
    jsonFlow.Initial.allows.forEach((element) => {
      importedEdges.find((x) => x.id == element.actionCode).source =
        jsonFlow.Initial.fromStateCode;
      importedEdges.find((x) => x.id == element.actionCode).target =
        element.allowedStateCode;
      var nextStepNode = importedNodes.find(
        (x) => x.id == element.allowedStateCode
      );

      const position = findFirstEmptyPosition(
        firstIdleNode.position.x + 350,
        firstIdleNode.position.y
      );
      nodesPosition.current.push(position);
      nextStepNode.position = position;
    });

    const initialNode = {
      id: (firstIdleNode.id - 1).toString(),
      type: "StartNode",
      position: { x: 0, y: 15 },
      data: {
        AddIdleNodeFunc: onAddIdleNodeFunc,
      },
    };
    importedNodes = [initialNode, ...importedNodes];

    const initialEdge = {
      id: (
        Math.min(...jsonFlow.Initial.allows.map((o) => o.actionCode)) - 1
      ).toString(),
      source: (firstIdleNode.id - 1).toString(),
      target: firstIdleNode.id.toString(),
      type: "custom-edge",
      data: { Name: "", noInput: true },
    };
    importedEdges = [initialEdge, ...importedEdges];

    jsonFlow.Steps.forEach((stepElement) => {
      var stepNode = importedNodes.find(
        (x) => x.id == stepElement.fromStateCode
      );
      stepNode.data.onEntry =
        stepElement.onEntry != undefined && stepElement.onEntry != ""
          ? stepElement.onEntry.name
          : "";
      stepNode.data.onExit =
        stepElement.onExit != undefined && stepElement.onExit != ""
          ? stepElement.onExit.name
          : "";
      if (stepNode.position == null) {
        stepNode.position = { x: 200, y: 0 };
      }
      if (stepElement.allows != undefined && stepElement.allows != "") {
        stepElement.allows.forEach((element) => {
          importedEdges.find((x) => x.id == element.actionCode).source =
            stepElement.fromStateCode;
          importedEdges.find((x) => x.id == element.actionCode).target =
            element.allowedStateCode;
          var nextStepNode = importedNodes.find(
            (x) => x.id == element.allowedStateCode
          );

          if (nextStepNode.position == null) {
            const position = findFirstEmptyPosition(
              stepNode.position.x + 350,
              stepNode.position.y
            );
            nodesPosition.current.push(position);
            nextStepNode.position = position;
          }
        });
      }
    });

    var maxNodeId = Math.max(...jsonFlow.States.map((o) => o.Code));
    var maxEdgeId = Math.max(...jsonFlow.Actions.map((o) => o.Code));
    var lastMaxId = maxNodeId > maxEdgeId ? maxNodeId : maxEdgeId;
    setId(lastMaxId);

    setEdges(importedEdges);
    setNodes(importedNodes);

    props.onSetWorkflowName(jsonFlow.Name);
  };

  const onNodesChange = useCallback(
    (changes) => {
      var startNode = nodes.find((x) => x.type === "StartNode");
      var firstIdleNodeId = edges.find((x) => x.source === startNode.id).target;
      if (
        changes[0].type === "remove" &&
        (changes[0].id === startNode.id || changes[0].id === firstIdleNodeId)
      ) {
        return;
      }
      setNodes((nds) => applyNodeChanges(changes, nds));
    },
    [setNodes]
  );

  const onEdgesChange = useCallback(
    (changes) => {
      var startNode = nodes.find((x) => x.type === "StartNode");
      var firstEdgeIdleNodeId = edges.find((x) => x.source === startNode.id).id;
      if (
        changes[0].type === "remove" &&
        changes[0].id === firstEdgeIdleNodeId
      ) {
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
      let edge = {
        id: getId(),
        source: connection.source,
        target: connection.target,
        type: "custom-edge",
        data: { Name: "" },
      };
      setEdges((eds) => addEdge(edge, eds));
    },
    [setEdges]
  );

  const onDeleteNode = (nodeId) => {
    const updatedNodes = nodes.filter((node) => node.id !== nodeId);
    setNodes(updatedNodes);
  };

  useEffect(() => {
    if (props.resetFlowCalled) {
      var resetNode = initialNodes;
      resetNode[1].data = { ...resetNode[1].data, reset: true };
      setNodes(initialNodes);
      setEdges(initialEdges);
      nodesPosition.current = [{ x: 0, y: 0 }];
      setId();
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
            onNodesDelete={onNodesDelete}
            // onEdgesDelete={}   // as needed
            onConnect={onConnect}
            onEdgeUpdate={onEdgeUpdate}
            onEdgeUpdateStart={onEdgeUpdateStart}
            onEdgeUpdateEnd={onEdgeUpdateEnd}
            onInit={setReactFlowInstance}
            nodeTypes={nodeTypes}
            edgeTypes={edgeTypes}
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
