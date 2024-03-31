import { useCallback, useState, useContext, useEffect } from "react";
import { Handle, Position, useStore } from "reactflow";
import { Button, Dropdown, ConfigProvider, Modal } from "antd";
import exitActionImg from "../Assets/exit-action.svg";
import entryActionImg from "../Assets/entry-action.svg";
import actionIconImg from "../Assets/action-icon.svg";
import uploadIconImg from "../Assets/upload-icon.svg";
import editIconImg from "../Assets/edit-icon.svg";
import trashImg from "../Assets/trash.svg";
import ApplicationContext from "../Store/ApplicationContext";

const NodeActvityModal = ({ node, isModalOpen, onApplyChanges }) => {
  const { AllFlowActivities, updateAllFlowActivities } =
    useContext(ApplicationContext);

  const [activities, setActivities] = useState({
    onEntryName: node.data.onEntry,
    onExitName: node.data.onExit,
  });
  const handleOk = () => {
    updateAllFlowActivities(flowActivitiesList);
    onApplyChanges(activities);
  };
  const handleCancel = () => {
    setFlowActivitiesList(AllFlowActivities);
    setActivities({
      onEntryName: node.data.onEntry,
      onExitName: node.data.onExit,
    });
    onApplyChanges(null, null);
  };

  const chooseOnEntryActionHandler = ({ key }) => {
    if (key == "registerActivity") {
      // setIsEntryActionSelected(true);
      uploadActivityDll();
    } else {
      setActivities({
        ...activities,
        onEntryName: items[0].children.find((x) => x.key == key).name,
      });
    }
  };

  const chooseOnExitActionHandler = ({ key }) => {
    if (key == "registerActivity") {
      // setIsEntryActionSelected(true);
      uploadActivityDll();
    } else {
      setActivities({
        ...activities,
        onExitName: items[0].children.find((x) => x.key == key).name,
      });
    }
  };

  const removeEntryActionHandler = () => {
    setActivities({ ...activities, onEntryName: "" });
  };
  const removeExitActionHandler = () => {
    setActivities({ ...activities, onExitName: "" });
  };
  const [flowActivitiesList, setFlowActivitiesList] =
    useState(AllFlowActivities);

    

    useEffect(() => {setFlowActivitiesList(AllFlowActivities)}, [AllFlowActivities])

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
      key: "registerActivity",
      label: (
        <span className="activities-dropdown-item">
          <img src={uploadIconImg} />
          <span className="activity-name">Register or update Activity</span>
        </span>
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
              Configure events for "{node.data.Name}"
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
            {activities.onEntryName == "" ? (
              <Dropdown
                menu={{
                  items,
                  onClick: chooseOnEntryActionHandler,
                }}
                placement="bottomRight"
                trigger={['click']}
              >
                <button className="add-actvity-btn">+ Add Actvity</button>
              </Dropdown>
            ) : (
              <div className="node-activity">
                <img src={actionIconImg} className="action-icon" />
                <p>{activities.onEntryName}</p>
                <span>
                  <Dropdown
                    menu={{
                      items,
                      onClick: chooseOnEntryActionHandler,
                    }}
                    placement="bottomRight"
                    trigger={['click']}
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
            {activities.onExitName == "" ? (
              <Dropdown
                menu={{
                  items,
                  onClick: chooseOnExitActionHandler,
                }}
                placement="bottomRight"
                trigger={['click']}
              >
                <button className="add-actvity-btn">+ Add Actvity</button>
              </Dropdown>
            ) : (
              <div className="node-activity">
                <img src={actionIconImg} className="action-icon" />
                <p>{activities.onExitName}</p>
                <span>
                  <Dropdown
                    menu={{
                      items,
                      onClick: chooseOnExitActionHandler,
                    }}
                    placement="bottomRight"
                    trigger={['click']}
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
