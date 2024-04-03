import { useState, useCallback } from "react";
import { ApiCaller } from "../ApiCaller"

const activityApiPath = "flowciao/api/activity"
const useActivityData = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const sendGetRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
     
        const response = await ApiCaller.getList(
            activityApiPath,
          requestConfig.params,
        );

        var data = [];
        if (response.status == 200) {
           data = response.data.map((activity) => ({
            key: activity.id,
            ...activity,
          }));
        }
        applyData(data);
      
    } catch (err) {
      setError(err.message || "Something went wrong!");
    }
    setIsLoading(false);
  }, []);

  
  const sendUploadDLLFileRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
        const response = await ApiCaller.post(
          `${activityApiPath}/register`,
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
    sendGetRequest,
    sendUploadDLLFileRequest
  };
};

export default useActivityData;
