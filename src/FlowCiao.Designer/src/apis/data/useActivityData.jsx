import { useState, useCallback } from "react";
import { ApiCaller } from "../ApiCaller"

const useActivityData = () => {

  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState(null);

  const sendGetRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
      if (requestConfig.retrieveSingleData) {

        const response = await ApiCaller.getOne("activities", {
          id: requestConfig.id,
          params: requestConfig.params,
        });
        var data = {};
        if (response.success) {
          data = {
            key: response.data.id,
            ...response.data,
          };
        }
        applyData(data);
      } else {
        const response = await ApiCaller.getList(
          "activities",
          requestConfig.params,
        );
        var data = [];
        if (response.success) {
          data = response.data.map((activity) => ({
            key: activity.id,
            ...activity,
          }));
        }
        applyData(data);
      }
    } catch (err) {
      setError(err.message || "Something went wrong!");
    }
    setIsLoading(false);
  }, []);

  
  const sendUploadDLLFileRequest = useCallback(async (requestConfig, applyData) => {
    setIsLoading(true);
    setError(null);
    try {
        const response = await DataProvider.post(
          "activities/upload",
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
