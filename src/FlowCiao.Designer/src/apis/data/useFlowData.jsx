import { useState, useCallback } from "react";
import { ApiCaller } from "../ApiCaller";

const activityApiPath = "flowciao/api/flows";
const useFlowData = () => {
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
      if (
        response !== undefined &&
        response.status == 200 &&
        response.data.status === "success" &&
        response.data !== undefined &&
        response.data.data !== undefined
      ) {
        data = response.data.data.map((flow) => ({
          key: flow.id,
          ...flow,
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

  return {
    isLoading,
    error,
    sendGetRequest,
  };
};

export default useFlowData;
