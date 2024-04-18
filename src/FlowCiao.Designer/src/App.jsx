import { useState, useRef, useEffect } from "react";
import Flow from "./Components/Flow";
import { ExclamationCircleFilled } from "@ant-design/icons";
import {
  ConfigProvider,
  Layout,
  Space,
  Input,
  Button,
  ColorPicker,
  Modal,
  Upload,
  Dropdown,
  message,
} from "antd";
import "./App.css";
import plusImg from "./Assets/plus.svg";
import importImg from "./Assets/import.svg";
import exportImg from "./Assets/export.svg";
import paintImg from "./Assets/paint.svg";
import publishImg from "./Assets/publish.svg";
import actionIconImg from "./Assets/action-icon.svg";
import uploadIconImg from "./Assets/upload-icon.svg";
import arrowDownIconImg from "./Assets/arrow-down-icon.svg";
import ApplicationContextProvider from "./Store/ApplicationContextProvider";
import useActivityData from "./apis/data/useActivityData";
import useFlowData from "./apis/data/useFlowData";
import useBuilderData from "./apis/data/useBuilderData";
const { Header, Content } = Layout;
const { confirm } = Modal;

function App() {
  const [messageApi, contextHolder] = message.useMessage();
  const flowDesignerRef = useRef();
  const [workflowName, setWorkflowName] = useState("My State Machine");

  const handleExportFlowAsJSON = () => {
    if (flowDesignerRef.current) {
      var json = flowDesignerRef.current.exportFlowAsJSON();
      downloadJSON(json);
    }
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
    a.download = workflowName + `_jsonFlow_${dateTime}.json`; // Set the filename here
    document.body.appendChild(a);
    a.click();
    URL.revokeObjectURL(url);
    document.body.removeChild(a);
  };

  const handleFileChange = (info) => {
    handleFileSelect(info.file);
  };
  const handleFileSelect = (file) => {
    const reader = new FileReader();
    reader.onload = () => {
      try {
        const jsonData = JSON.parse(reader.result);
        if (flowDesignerRef.current) {
          var err = flowDesignerRef.current.importJson(jsonData);
          if (err != undefined && err != "") {
            messageApi.open({
              type: "error",
              content: err,
            });
          }
        }
      } catch (error) {
        console.error("Error parsing JSON file:", error);
      }
    };
    reader.readAsText(file);
  };

  const {
    isLoading: isBuilderLoading,
    error: builderError,
    sendBuildFlowRequest,
  } = useBuilderData();

  const onBuildClick = () => {
    if (flowDesignerRef.current) {
      var json = flowDesignerRef.current.exportFlowAsJSON();
      sendBuildFlowRequest(
        {
          params: { Content: json },
        },
        (response) => {
          if (response.status == 200 && response.data.status === "success") {
            messageApi.open({
              type: "success",
              content: "Building is successful",
            });
          } else {
            messageApi.open({
              type: "error",
              content: response.data.message,
            });
          }
        }
      );
    }
  };

  const {
    isLoading,
    error,
    sendGetRequest: senGetActivitiesRequest,
    sendUploadDLLFileRequest: sendUploadActvitiesDllFileRequest,
  } = useActivityData();
  const [flowActivitiesList, setFlowActivitiesList] = useState([]);

  const [isUploadingDll, setIsUploadingDll] = useState(false);

  const handleActivityDllFileChange = (info) => {
    const fileReader = new FileReader();
    fileReader.onload = (e) => {
      const formDatas = new FormData();
      formDatas.append("file", info.file);
      setIsUploadingDll(true);
      sendUploadActvitiesDllFileRequest(
        {
          params: formDatas,
        },
        (response) => {
          setIsUploadingDll(false);

          if (response.success) {
            messageApi.open({
              type: "success",
              content: "Uploading is successful",
            });
            getActivities();
          } else {
            messageApi.open({
              type: "error",
              content: "Uploading is failed",
            });
          }
        }
      );
    };

    fileReader.readAsArrayBuffer(info.file);
  };

  const actionDropdownOnClick = (isOpen) => {
    if (isOpen) {
      getActivities();
    }
  };
  const getActivities = () => {
    senGetActivitiesRequest({}, (flowActivities) => {
      setFlowActivitiesList(flowActivities);
    });
  };

  var registerActivityDropDownItem = [
    {
      key: "headerRegisterActivity",
      label: (
        <Upload
          accept=".dll"
          onChange={handleActivityDllFileChange}
          showUploadList={false}
          beforeUpload={() => false} // Prevent auto-upload
        >
          <span className="activities-dropdown-item">
            <img src={uploadIconImg} />
            <span className="activity-name">Register or update Activity</span>
          </span>
        </Upload>
      ),
    },
  ];
  var activitiesDropDownItems = [];

  if (flowActivitiesList.length > 0) {
    activitiesDropDownItems = [
      {
        key: "headerActivities",
        type: "group",
        label: "Custom Activities",
        overlayclassname: "activities-dropdown-items-container",
        children: flowActivitiesList.map((x, index) => ({
          key: index + 1,
          name: x.name,
          label: (
            <span className="activities-dropdown-item">
              <img src={actionIconImg} />
              <span className="activity-name">{x.name}</span>
            </span>
          ),
        })),
      },
      {
        type: "divider",
      },
    ];
  }

  activitiesDropDownItems = activitiesDropDownItems.concat(
    registerActivityDropDownItem
  );

  const [color, setColor] = useState("#1677ff");
  const onChangeColor = (selectedColor) => {
    setColor(selectedColor.toHexString());
    let element = document.getElementsByClassName("react-flow__node-idleNode");
    for (let i = 0; i < element.length; i++) {
      element[i].firstChild.style.setProperty(
        "border-top-color",
        selectedColor.toHexString(),
        "important"
      );
    }
  };

  const {
    isFlowLoading,
    flowError,
    sendGetRequest: senGetFlowsRequest,
  } = useFlowData();
  const [previousFlows, setPreviousFlow] = useState([]);

  const getWorkflows = () => {
    senGetFlowsRequest({}, (data) => {
      var flowActivities = data.map((flow) => ({
        name: flow.name,
        json: flow.serializedJson,
      }));
      setPreviousFlow(flowActivities);
    });
  };

  const loadFlow = (serializedJson) => {
    try {
      const jsonData = JSON.parse(serializedJson);
      if (flowDesignerRef.current) {
        var err = flowDesignerRef.current.importJson(jsonData);
        if (err != undefined && err != "") {
          messageApi.open({
            type: "error",
            content: "Loading flow is failed",
          });
        }
      }
    } catch (error) {
      console.error("Error parsing JSON file:", error);
    }
  };

  var createNewFlowDropDownItem = [
    {
      key: "newFlow",
      label: (
        <span className="activities-dropdown-item">
          <img src={plusImg} />
          <span className="activity-name">New State Machine</span>
        </span>
      ),
    },
  ];
  var workflowItems = [];
  if (previousFlows.length > 0) {
    workflowItems = [
      {
        key: "flows",
        type: "group",
        label: "Previous State Machines",
        overlayclassname: "activities-dropdown-items-container",
        children: previousFlows.map((x, index) => ({
          key: index + 1,
          name: x.name,
          label: (
            <div
              className="activities-dropdown-item"
              onClick={() => {
                loadFlow(x.json);
              }}
            >
              <img src={actionIconImg} />
              <span className="activity-name">{x.name}</span>
            </div>
          ),
        })),
      },
      {
        type: "divider",
      },
    ];
  }

  workflowItems = workflowItems.concat(createNewFlowDropDownItem);

  const chooseWorkflowHandler = ({ key }) => {
    if (key == "newFlow") {
      resetFlowClick();
    }
  };

  const [resetFlow, setResetFlow] = useState(false);
  const resetFlowClick = (isCleared = true) => {
    if (isCleared) {
      showConfirm();
    } else {
      setResetFlow(false);
    }
  };
  const showConfirm = () => {
    confirm({
      title: "Do you want to delete flow?",
      icon: <ExclamationCircleFilled />,
      // content: 'Some descriptions',
      onOk() {
        setWorkflowName("My State Machine");
        setResetFlow(true);
      },
    });
  };

  const workflowNameOnChange = (event) => {
    setWorkflowName(event.target.value);
  };

  const [focused, setFocused] = useState(false);
  const workflowNameHandleBlur = () => {
    setFocused(false);
  };

  const workflowNameHandleFocus = () => {
    setFocused(true);
  };

useEffect(() => {
  const errorHandler = (e) => {
    if (
      e.message.includes(
        "ResizeObserver loop completed with undelivered notifications" ||
          "ResizeObserver loop limit exceeded"
      )
    ) {
      const resizeObserverErr = document.getElementById(
        "webpack-dev-server-client-overlay"
      );
      if (resizeObserverErr) {
        resizeObserverErr.style.display = "none";
      }
    }
  };
  window.addEventListener("error", errorHandler);

  return () => {
    window.removeEventListener("error", errorHandler);
  };
}, []);

  return (
    <ApplicationContextProvider color={color}>
      <ConfigProvider
        theme={{
          components: {
            Layout: {
              headerBg: "#fff",
            },
          },
        }}
      >
        {contextHolder}
        <Layout className="main-layout">
          <Header className="main-header">
            <Space className="header-workflow-name-container">
              <Input
                placeholder="Workflow Name"
                id="workflow-name"
                className={`${focused ? "focused" : ""}`}
                value={workflowName}
                onChange={workflowNameOnChange}
                onBlur={workflowNameHandleBlur}
                onFocus={workflowNameHandleFocus}
              />
              <Dropdown
                menu={{
                  items: workflowItems,
                  onClick: chooseWorkflowHandler,
                }}
                placement="bottomRight"
                trigger={["click"]}
                onOpenChange={(isOpen) => {
                  if (isOpen) {
                    getWorkflows();
                  }
                }}
              >
                <Button className="header-workflow-dropdown-btn">
                  <img src={arrowDownIconImg} width={12} />
                </Button>
              </Dropdown>
            </Space>
            <Space className="header-botton-container">
              <Button
                loading={isBuilderLoading}
                className="header-btn"
                style={{ background: "#0047FF" }}
                icon={<img src={publishImg} width={18} />}
                onClick={onBuildClick}
              />

              <Button
                className="header-btn"
                icon={<img src={exportImg} width={18} />}
                onClick={handleExportFlowAsJSON}
              />
              <Upload
                accept=".json"
                className="header-btn"
                onChange={handleFileChange}
                showUploadList={false}
                beforeUpload={() => false} // Prevent auto-upload
              >
                <Button
                  className="header-btn"
                  icon={<img src={importImg} width={18} />}
                />
              </Upload>
              <Dropdown
                menu={{
                  items: activitiesDropDownItems,
                }}
                placement="bottomRight"
                trigger={["click"]}
                onOpenChange={actionDropdownOnClick}
              >
                <Button
                  loading={isUploadingDll}
                  className="header-btn header-activities-btn"
                >
                  <img className="activities-icon" src={actionIconImg} />
                  <img src={arrowDownIconImg} />
                </Button>
              </Dropdown>
              <ColorPicker defaultValue={color} onChange={onChangeColor}>
                <Button
                  className="header-btn"
                  icon={<img src={paintImg} width={22} />}
                />
              </ColorPicker>
            </Space>
          </Header>
          <Content className="main-content">
            <div>
              <Flow
                ref={flowDesignerRef}
                resetFlowCalled={resetFlow}
                onResetFlowClick={resetFlowClick}
                workflowName={workflowName}
                onSetWorkflowName={(name) => {
                  setWorkflowName(name);
                }}
              />
            </div>
          </Content>
        </Layout>
      </ConfigProvider>
    </ApplicationContextProvider>
  );
}

export default App;
