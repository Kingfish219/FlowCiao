import { useState, useRef } from "react";
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
const { Header, Content } = Layout;
const { confirm } = Modal;

function App() {
  const flowDesignerRef = useRef();
  const [workflowName, setWorkflowName] = useState("My State Machine");

  const handleExportFlowAsJSON = () => {
    if (flowDesignerRef.current) {
      flowDesignerRef.current.exportFlowAsJSON();
    }
  };

  const handleFileChange = (info) => {
    // if (info.file.status === "done") {
    handleFileSelect(info.file);
    //  }
  };
  const handleFileSelect = (file) => {
    const reader = new FileReader();
    reader.onload = () => {
      try {
        const jsonData = JSON.parse(reader.result);
        if (flowDesignerRef.current) {
          flowDesignerRef.current.importJson(jsonData);
        }
      } catch (error) {
        console.error("Error parsing JSON file:", error);
      }
    };
    reader.readAsText(file);
  };

  const chooseActionHandler = ({ key }) => {
    if (key == "headerRegisterActivity") {
      uploadActivityDll();
    }
  };
  const [flowActivitiesList, setFlowActivitiesList] = useState([]);

  const actionDropdownOnClick = () => {
    getActivities();
  }
  const uploadActivityDll = () => {
    getActivities();
  };
  const getActivities = () => {
    var flowActivities = [
      {
        name: "HelloWordActivity",
      },
    ];
    setFlowActivitiesList(flowActivities);
  };

  var registerActivityDropDownItem = [
    {
      key: "headerRegisterActivity",
      label: (
        <span className="activities-dropdown-item">
          <img src={uploadIconImg} />
          <span className="activity-name">Register or update Activity</span>
        </span>
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

  const [previousFlows, setPreviousFlow] = useState([]);
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
                trigger={['click']}
              >
                <Button className="header-workflow-dropdown-btn">
                  <img src={arrowDownIconImg} width={12} />
                </Button>
              </Dropdown>
            </Space>
            <Space className="header-botton-container">
              <Button
                className="header-btn"
                style={{ background: "#0047FF" }}
                icon={<img src={publishImg} width={18} />}
              />

              <Button
                className="header-btn"
                icon={<img src={exportImg} width={18} />}
                onClick={handleExportFlowAsJSON}
              />
              <Upload
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
                  onClick: chooseActionHandler,
                }}
                placement="bottomRight"
                trigger={['click']}
                onClick={actionDropdownOnClick}
              >
                <Button className="header-btn header-activities-btn">
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
              {/* <Button icon={<img src={plusImg} />} onClick={resetFlowClick} /> */}
            </Space>
          </Header>
          <Content className="main-content">
            <div>
              <Flow
                ref={flowDesignerRef}
                resetFlowCalled={resetFlow}
                onResetFlowClick={resetFlowClick}
                workflowName={workflowName}
              />
            </div>
          </Content>
        </Layout>
      </ConfigProvider>
    </ApplicationContextProvider>
  );
}

export default App;
