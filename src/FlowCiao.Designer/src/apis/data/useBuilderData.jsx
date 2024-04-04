import { useState, useCallback } from "react";
import { ApiCaller } from "../ApiCaller"

const activityApiPath = "flowciao/api/builder"
const useBuilderData = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const sendBuildFlowRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
        const response = await ApiCaller.post(
          `${activityApiPath}/json`,
          requestConfig.params,
        );        
        applyData(response);
      
    } catch (err) {
        setError(err.message || "Something went wrong!");
        var errorResponse = {success: false, message: (err.message || "Something went wrong!")}
        applyData(errorResponse);
    }
    setIsLoading(false);
  }, []);

  
  return {
    isLoading,
    error,
    sendBuildFlowRequest
  };
};

export default useBuilderData;
