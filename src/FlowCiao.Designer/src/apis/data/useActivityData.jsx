import { useState, useCallback } from "react";
import { ApiCaller } from "../ApiCaller";

const activityApiPath = "flowciao/api/activity";
const useActivityData = () => {
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const sendGetRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
      const response = await ApiCaller.getList(
        activityApiPath,
        requestConfig.params
      );
      var data = [];
      if (response.status == 200 && response.data.status === "success") {
        data = response.data.data.map((activity) => ({
          key: activity.id,
          ...activity,
        }));
      }
      applyData(data);
    } catch (err) {
      setError(
        err.response.data.message || err.message || "Something went wrong!"
      );
    }
    setIsLoading(false);
  }, []);

  const sendUploadDLLFileRequest = useCallback(
    async (requestConfig, applyData) => {
      setIsLoading(true);
      setError(null);
      var result = { success: false, message: "" };
      try {
        const response = await ApiCaller.post(
          `${activityApiPath}/register`,
          requestConfig.params
        );
        if (response.status == 200 && response.data.status === "success") {
          result.success = true;
        } else {
          result.success = false;
          result.message = response.data.message || response.message;
        }
        applyData(result);
      } catch (err) {
        setError(err.message || "Something went wrong!");
        result.success = false;
        result.message = err.response.data.message || err.message || "Something went wrong!";
        applyData(result);
      }
      setIsLoading(false);
    },
    []
  );

  return {
    isLoading,
    error,
    sendGetRequest,
    sendUploadDLLFileRequest,
  };
};

export default useActivityData;
