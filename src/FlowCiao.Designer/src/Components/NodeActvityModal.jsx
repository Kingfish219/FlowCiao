import { useState } from "react";
import {
  Dropdown,
  ConfigProvider,
  Modal,
  message,
  Button,
  Upload,
  Spin,
} from "antd";
import { LoadingOutlined } from '@ant-design/icons';
import actionIconImg from "../Assets/action-icon.svg";
import uploadIconImg from "../Assets/upload-icon.svg";
import editIconImg from "../Assets/edit-icon.svg";
import trashImg from "../Assets/trash.svg";
import useActivityData from "../apis/data/useActivityData";

const NodeActvityModal = ({ node, isModalOpen, onApplyChanges }) => {
  const [messageApi, contextHolder] = message.useMessage();
  const [activities, setActivities] = useState({
    onEntryName: node.data.onEntry.name,
    onEntryActorName: node.data.onEntry.actorName,
    onExitName: node.data.onExit.name,
    onExitActorName: node.data.onExit.actorName,
  });
  const handleOk = () => {
    onApplyChanges(activities);
  };
  const handleCancel = () => {
    setActivities({
      onEntryName: node.data.onEntry.name,
      onEntryActorName: node.data.onEntry.actorName,
      onExitName: node.data.onExit.name,
      onExitActorName: node.data.onExit.actorName,
    });
    onApplyChanges(null);
  };

  const chooseOnEntryActionHandler = ({ key }) => {
    if (key == "registerActivity") {
      uploadActivityDll();
    } else {
      setActivities({
        ...activities,
        onEntryName: items[0].children.find((x) => x.key == key).name,
        onEntryActorName: items[0].children.find((x) => x.key == key).actorName,
      });
    }
  };

  const chooseOnExitActionHandler = ({ key }) => {
    if (key != "registerActivity") {
      setActivities({
        ...activities,
        onExitName: items[0].children.find((x) => x.key == key).name,
        onExitActorName: items[0].children.find((x) => x.key == key).actorName,
      });
    }
  };

  const removeEntryActionHandler = () => {
    setActivities({ ...activities, onEntryName: "", onEntryActorName: "" });
  };
  const removeExitActionHandler = () => {
    setActivities({ ...activities, onExitName: "" , onExitActorName: ""});
  };

  const uploadActivityDll = () => {
    getActivities();
  };

  const {
    isLoading,
    error,
    sendGetRequest: senGeActivitiesRequest,
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
              content: "Uploaded successfully",
            });
            getActivities();
          }else{
            messageApi.open({
              type: "error",
              content: response.message,
            });
          }
        }
      );
    };

    fileReader.readAsArrayBuffer(info.file);
  };

  const [actionDropDownTarget, setActionDropDownTarget] = useState("")
  const actionDropdownOnClick = (isOpen, target) => {
    if (isOpen) {
      setActionDropDownTarget(target)
      getActivities();
    }else{
    setActionDropDownTarget("")
    }
  };
  const getActivities = () => {
    senGeActivitiesRequest({}, (flowActivities) => {
      setFlowActivitiesList(flowActivities);
    });
  };

  var registerActivityDropDownItem = [
    {
      key: "registerActivity",
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
  var items = [];
  if (flowActivitiesList.length > 0) {
    items = [
      {
        key: "activities",
        type: "group",
        label: "Custom Activities",
        overlayclassname: "activities-dropdown-items-container",
        children: flowActivitiesList.map((x, index) => ({
          key: index + 1,
          name: x.name,
          actorName: x.actorName,
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

  items = items.concat(registerActivityDropDownItem);
  return (
    <>
      {contextHolder}
      <ConfigProvider
        theme={{
          components: {
            Modal: {
              wireframe: true,
            },
          },
        }}
      >
        <Modal
          title={
            <span className="modal-title">
              Configure events for "{(node.data.Name == "" ? "New State" : node.data.Name)}"
            </span>
          }
          open={isModalOpen}
          onOk={handleOk}
          onCancel={handleCancel}
          okText="Apply Changes"
          bodyStyle={{ height: "310px" }}
          okButtonProps={{ className: "action-modal-footer-btn" }}
          cancelButtonProps={{
            className: "action-modal-footer-btn",
            style: {
              border: "unset",
            },
          }}
          className="activity-modal"
        >
          <p className="title">On Entry</p>
          <div className="activity-container dash-bottom-border">
            {activities.onEntryName == "" ||  activities.onEntryName == undefined ? (
              <Dropdown
                menu={{
                  items,
                  onClick: chooseOnEntryActionHandler,
                }}
                placement="bottomRight"
                trigger={["click"]}
                onOpenChange={(isOpen) => actionDropdownOnClick(isOpen, "onEntryBtn")}
              >
                <Button loading={isUploadingDll && actionDropDownTarget == "onEntryBtn"} className="add-actvity-btn">
                  + Add Actvity
                </Button>
              </Dropdown>
            ) : (
              <div className="node-activity">
                <img src={actionIconImg} className="action-icon" />
                <p>{activities.onEntryName}</p>
                <span>
                {(isUploadingDll && actionDropDownTarget == "onEditEntryBtn") && <Spin
                    indicator={
                      <LoadingOutlined
                        style={{
                          fontSize: 14,
                          margin: "0 8px 8px 0"
                        }}
                        spin
                      />
                    }
                  />}
                  <Dropdown
                    menu={{
                      items,
                      onClick: chooseOnEntryActionHandler,
                    }}
                    placement="bottomRight"
                    trigger={["click"]}
                    onOpenChange={(isOpen) => actionDropdownOnClick(isOpen, "onEditEntryBtn")}
                  >
                    <button className="node-action-edit-btn">
                      <img src={editIconImg} />
                    </button>
                  </Dropdown>
                  <button
                    className="node-action-remove-btn"
                    onClick={removeEntryActionHandler}
                  >
                    <img src={trashImg} />
                  </button>
                </span>
              </div>
            )}
          </div>
          <p className="title">On Exit</p>
          <div className="activity-container">
            {activities.onExitName == "" || activities.onExitName == undefined ? (
              <Dropdown
                menu={{
                  items,
                  onClick: chooseOnExitActionHandler,
                }}
                placement="bottomRight"
                trigger={["click"]}
                onOpenChange={(isOpen) => actionDropdownOnClick(isOpen, "onExitBtn")}
              >
                <Button loading={isUploadingDll && actionDropDownTarget == "onExitBtn"} className="add-actvity-btn">
                  + Add Actvity
                </Button>
              </Dropdown>
            ) : (
              <div className="node-activity">
                <img src={actionIconImg} className="action-icon" />
                <p>{activities.onExitName}</p>
                <span>
                  {(isUploadingDll  && actionDropDownTarget == "onEditExitBtn") && <Spin
                    indicator={
                      <LoadingOutlined
                        style={{
                          fontSize: 14,
                          margin: "0 8px 8px 0"
                        }}
                        spin
                      />
                    }
                  />}
                  <Dropdown
                    menu={{
                      items,
                      onClick: chooseOnExitActionHandler,
                    }}
                    placement="bottomRight"
                    trigger={["click"]}
                    onOpenChange={(isOpen) => actionDropdownOnClick(isOpen, "onEditExitBtn")}
                  >
                    <button className="node-action-edit-btn">
                      <img src={editIconImg} />
                    </button>
                  </Dropdown>
                  <button
                    className="node-action-remove-btn"
                    onClick={removeExitActionHandler}
                  >
                    <img src={trashImg} />
                  </button>
                 
                </span>
              </div>
            )}
          </div>
        </Modal>
      </ConfigProvider>
    </>
  );
};
export default NodeActvityModal;
