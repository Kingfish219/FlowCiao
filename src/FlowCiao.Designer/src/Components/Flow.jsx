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
        onEntry: { name: "", actorName: "" },
        onExit: { name: "", actorName: "" },
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
        onEntry: { name: "", actorName: "" },
        onExit: { name: "", actorName: "" },
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
      triggers: [...edges],
    };
    var startNode = nodes.find((x) => x.type === "StartNode");
    var firstIdleNodeId = edges.find((x) => x.source === startNode.id).target;

    var states = nodes
      .filter((x) => x.id !== startNode.id)
      .map((node) => ({
        Code: node.id,
        Name: node.data.Name === "" ? "New State" : node.data.Name,
      }));

    var triggers = edges
      .filter((x) => x.source !== startNode.id)
      .map((edge) => ({
        Code: edge.id,
        Name: edge.data.Name === "" ? "New Trigger" : edge.data.Name,
      }));

    var initial = {
      fromStateCode: firstIdleNodeId,
      allows: edges
        .filter((edge) => edge.source === firstIdleNodeId)
        .map((item) => ({
          allowedStateCode: item.target,
          triggerCode: item.id,
        })),
      onEntry:
        nodes.find((x) => x.id === firstIdleNodeId).data.onEntry.name != ""
          ? {
              name: nodes.find((x) => x.id === firstIdleNodeId).data.onEntry
                .name,
              actorName: nodes.find((x) => x.id === firstIdleNodeId).data
                .onEntry.actorName,
            }
          : { name: "", actorName: "" },
      onExit:
        nodes.find((x) => x.id === firstIdleNodeId).data.onExit.name != ""
          ? {
              name: nodes.find((x) => x.id === firstIdleNodeId).data.onExit
                .name,
              actorName: nodes.find((x) => x.id === firstIdleNodeId).data.onExit
                .actorName,
            }
          : { name: "", actorName: "" },
    };

    var steps = edges
      .filter((x) => x.source !== startNode.id && x.source !== firstIdleNodeId)
      .map((item) => ({
        fromStateCode: item.source,
        allows: edges
          .filter((edge) => edge.source === item.source)
          .map((item2) => ({
            allowedStateCode: item2.target,
            triggerCode: item2.id,
          })),
        onEntry:
          nodes.find((x) => x.id === item.source).data.onEntry.name != ""
            ? {
                name: nodes.find((x) => x.id === item.source).data.onEntry.name,
                actorName: nodes.find((x) => x.id === item.source).data.onEntry
                  .actorName,
              }
            : { name: "", actorName: "" },
        onExit:
          nodes.find((x) => x.id === item.source).data.onExit.name != ""
            ? {
                name: nodes.find((x) => x.id === item.source).data.onExit.name,
                actorName: nodes.find((x) => x.id === item.source).data.onExit
                  .actorName,
              }
            : { name: "", actorName: "" },
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
          nodes.find((x) => x.id === item.Code).data.onEntry.name != ""
            ? {
                name: nodes.find((x) => x.id === item.Code).data.onEntry.name,
                actorName: nodes.find((x) => x.id === item.Code).data.onEntry
                  .actorName,
              }
            : { name: "", actorName: "" },
        onExit:
          nodes.find((x) => x.id === item.Code).data.onExit.name != ""
            ? {
                name: nodes.find((x) => x.id === item.Code).data.onExit.name,
                actorName: nodes.find((x) => x.id === item.Code).data.onExit
                  .actorName,
              }
            : { name: "", actorName: "" },
      }));

    const flow = {
      Key: props.workflowName,
      Name: props.workflowName,
      States: states,
      Triggers: triggers,
      Initial: initial,
      Steps: steps.concat(endSteps),
    };

    const jsonFlow = JSON.stringify(flowData, null);
    // downloadJSON(JSON.stringify(flow, null));
    return JSON.stringify(flow, null);
    // console.log("Exported JSON:", jsonFlow);
    // console.log("Exported JSON22: ", JSON.stringify(flow, null));
  };

  function ConvertToCamelCase(o) {
    var newO, origKey, newKey, value;
    if (o instanceof Array) {
      return o.map(function (value) {
        if (typeof value === "object") {
          value = ConvertToCamelCase(value);
        }
        return value;
      });
    } else {
      newO = {};
      for (origKey in o) {
        if (o.hasOwnProperty(origKey)) {
          newKey = (
            origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey
          ).toString();
          value = o[origKey];
          if (
            value instanceof Array ||
            (value !== null && value.constructor === Object)
          ) {
            value = ConvertToCamelCase(value);
          }
          newO[newKey] = value;
        }
      }
    }
    return newO;
  }

  const importJson = (jsonFlow) => {
    try {
      jsonFlow = ConvertToCamelCase(jsonFlow);
      nodesPosition.current = [{ x: 0, y: 0 }];
      setId();

      var importedNodes = jsonFlow.states.map((node) => ({
        id: node.code.toString(),
        type: "idleNode",
        data: {
          AddIdleNodeFunc: onAddIdleNodeFunc,
          Name: node.name,
          onEntry: { name: "", actorName: "" },
          onExit: { name: "", actorName: "" },
          reset: true,
        },
        origin: [0.5, 0.0],
        position: null,
      }));

      var importedEdges = jsonFlow.triggers.map((edge) => ({
        id: edge.code.toString(),
        source: "",
        target: "",
        type: "custom-edge",
        data: { Name: edge.name },
      }));

      var firstIdleNode = importedNodes.find(
        (x) => x.id == jsonFlow.initial.fromStateCode.toString()
      );
      firstIdleNode.data.onEntry =
        jsonFlow.initial.onEntry != undefined &&
        jsonFlow.initial.onEntry.name != ""
          ? {
              name: jsonFlow.initial.onEntry.name,
              actorName: jsonFlow.initial.onEntry.actorName,
            }
          : { name: "", actorName: "" };
      firstIdleNode.data.onExit =
        jsonFlow.initial.onExit != undefined &&
        jsonFlow.initial.onExit.name != ""
          ? {
              name: jsonFlow.initial.onExit.name,
              actorName: jsonFlow.initial.onExit.actorName,
            }
          : { name: "", actorName: "" };
      firstIdleNode.position = { x: 200, y: 0 };
      jsonFlow.initial.allows.forEach((element) => {
        importedEdges.find(
          (x) => x.id == element.triggerCode.toString()
        ).source = jsonFlow.initial.fromStateCode.toString();
        importedEdges.find(
          (x) => x.id == element.triggerCode.toString()
        ).target = element.allowedStateCode.toString();
        var nextStepNode = importedNodes.find(
          (x) => x.id == element.allowedStateCode.toString()
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
          Math.min(...jsonFlow.initial.allows.map((o) => o.triggerCode)) - 1
        ).toString(),
        source: (firstIdleNode.id - 1).toString(),
        target: firstIdleNode.id.toString(),
        type: "custom-edge",
        data: { Name: "", noInput: true },
      };
      importedEdges = [initialEdge, ...importedEdges];

      jsonFlow.steps.forEach((stepElement) => {
        var stepNode = importedNodes.find(
          (x) => x.id == stepElement.fromStateCode.toString()
        );
        stepNode.data.onEntry =
          stepElement.onEntry != undefined && stepElement.onEntry.name != ""
            ? {
                name: stepElement.onEntry.name,
                actorName: stepElement.onEntry.actorName,
              }
            : { name: "", actorName: "" };
        stepNode.data.onExit =
          stepElement.onExit != undefined && stepElement.onExit.name != ""
            ? {
                name: stepElement.onExit.name,
                actorName: stepElement.onExit.actorName,
              }
            : { name: "", actorName: "" };
        if (stepNode.position == null) {
          stepNode.position = { x: 200, y: 0 };
        }
        if (stepElement.allows != undefined && stepElement.allows != "") {
          stepElement.allows.forEach((element) => {
            importedEdges.find(
              (x) => x.id == element.triggerCode.toString()
            ).source = stepElement.fromStateCode.toString();
            importedEdges.find(
              (x) => x.id == element.triggerCode.toString()
            ).target = element.allowedStateCode.toString();
            var nextStepNode = importedNodes.find(
              (x) => x.id == element.allowedStateCode.toString()
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

      var maxNodeId = Math.max(...jsonFlow.states.map((o) => o.code));
      var maxEdgeId = Math.max(...jsonFlow.triggers.map((o) => o.code));
      var lastMaxId = maxNodeId > maxEdgeId ? maxNodeId : maxEdgeId;
      setId(lastMaxId);

      setEdges(importedEdges);
      setNodes(importedNodes);

      props.onSetWorkflowName(jsonFlow.name);
    } catch (err) {
      return "Importing json is failed.";
    }
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

  const { setViewport } = useReactFlow();

  useEffect(() => {
    if (props.resetFlowCalled) {
      var resetNode = initialNodes;
      resetNode[1].data = { ...resetNode[1].data, reset: true };
      setNodes(initialNodes);
      setEdges(initialEdges);
      nodesPosition.current = [{ x: 0, y: 0 }];
      setId();
      setViewport(
        {
          x: window.innerWidth * 0.3,
          y: window.innerHeight * 0.3,
          zoom: 1.3,
        },
        { duration: 800 }
      );
      props.onResetFlowClick(false);
    }
  }, [props.resetFlowCalled]);

  const connectionLineStyle = {
    strokeWidth: 1.5,
    stroke: "#b1b1b7",
  };

  return (
    // <div className="dndflow">
    //   <ReactFlowProvider>
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
        // onEdgeUpdate={onEdgeUpdate}
        // onEdgeUpdateStart={onEdgeUpdateStart}
        // onEdgeUpdateEnd={onEdgeUpdateEnd}
        onInit={setReactFlowInstance}
        nodeTypes={nodeTypes}
        edgeTypes={edgeTypes}
        connectionLineComponent={CustomConnectionLine}
        connectionLineStyle={connectionLineStyle}
        // fitView
        defaultViewport={{
          x: window.innerWidth * 0.3,
          y: window.innerHeight * 0.3,
          zoom: 1.3,
        }}
      >
        <Controls />
      </ReactFlow>
    </div>
  );
});

export default forwardRef(
  (
    { resetFlowCalled, onResetFlowClick, workflowName, onSetWorkflowName },
    ref
  ) => (
    <div className="dndflow">
      <ReactFlowProvider>
        <Flow
          ref={ref}
          resetFlowCalled = {resetFlowCalled}
          onResetFlowClick = {onResetFlowClick}
          workflowName = {workflowName}
          onSetWorkflowName = {onSetWorkflowName}
        />
      </ReactFlowProvider>
    </div>
  )
);
