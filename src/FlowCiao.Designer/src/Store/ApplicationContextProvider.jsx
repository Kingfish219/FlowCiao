import React, { useState,useEffect } from 'react';
import ApplicationContext from './ApplicationContext';

 const ApplicationContextProvider = ({ color, activities, children }) => {

  const [allFlowActivities, setAllFlowActivities] = useState([]);

  useEffect(() => {
    setAllFlowActivities(activities);
  }, [activities]);
  
  const updateAllFlowActivities = (newActivities) => {
    setAllFlowActivities(newActivities);
  };

  const contextValue = {
    Theme: { borderColor: color },
    AllFlowActivities: allFlowActivities,
    updateAllFlowActivities,
  };
  return (
    <ApplicationContext.Provider value={contextValue}>
      {children}
    </ApplicationContext.Provider>
  );
};


export default ApplicationContextProvider;