import React, { useState,useEffect } from 'react';
import ApplicationContext from './ApplicationContext';

 const ApplicationContextProvider = ({ color, children }) => {

  const contextValue = {
    Theme: { borderColor: color },
  };
  return (
    <ApplicationContext.Provider value={contextValue}>
      {children}
    </ApplicationContext.Provider>
  );
};


export default ApplicationContextProvider;