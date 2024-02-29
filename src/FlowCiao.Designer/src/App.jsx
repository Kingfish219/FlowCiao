import { useState, useRef } from "react";
import Flow from "./Components/Flow";
import {
  ExclamationCircleFilled
} from "@ant-design/icons";
import {
  ConfigProvider,
  Layout,
  Space,
  Input,
  Button,
  ColorPicker,
  Modal
} from "antd";
import "./App.css";
import plusImg from "./Assets/plus.svg";
import importImg from "./Assets/import.svg";
import exportImg from "./Assets/export.svg";
import paintImg from "./Assets/paint.svg";
import publishImg from "./Assets/publish.svg";
import ThemeContext from "./Store/ThemeContext";
const { Header, Footer, Sider, Content } = Layout;
const { confirm } = Modal;

function App() {
  const flowDesignerRef = useRef();
  const [workflowName, setWorkflowName] = useState("");

  const handleExportFlowAsJSON = () => {
    if (flowDesignerRef.current) {
      flowDesignerRef.current.exportFlowAsJSON();
    }
  };

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
  const [resetFlow, setResetFlow] = useState(false);
  const resetFlowClick = (isCleared = true) =>{
    if(isCleared) {
      showConfirm();
    }else{
      setResetFlow(false)
    }
  }
  const showConfirm = () => {
    confirm({
      title: 'Do you want to delete flow?',
      icon: <ExclamationCircleFilled />,
      // content: 'Some descriptions',
      onOk() {
        setWorkflowName("")
      setResetFlow(true)
      }
    });
  };

  const workflowNameOnChange = (event) => {
    setWorkflowName(event.target.value);
  }

  return (
    <ThemeContext.Provider
      value={{
        borderColor: color,
      }}
    >
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
            <Input placeholder="Workflow Name" id="workflow-name" value={workflowName} onChange={workflowNameOnChange}/>
            <Space className="header-botton-container">
              <Button
                style={{ background: "#0047FF" }}
                icon={<img src={publishImg} />}
              />

              <ColorPicker defaultValue={color} onChange={onChangeColor}>
                <Button icon={<img src={paintImg} />} />
              </ColorPicker>
              <Button
                icon={<img src={exportImg} />}
                onClick={handleExportFlowAsJSON}
              />
              <Button icon={<img src={importImg} />} />
              <Button icon={<img src={plusImg} />} onClick={resetFlowClick}/>
            </Space>
          </Header>
          <Content className="main-content">
            <div>
              <Flow ref={flowDesignerRef} resetFlowCalled={resetFlow} onResetFlowClick={resetFlowClick}/>
            </div>
          </Content>
        </Layout>
      </ConfigProvider>
    </ThemeContext.Provider>
  );
}

export default App;
