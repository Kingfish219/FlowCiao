import React from 'react';

const ApplicationContext = React.createContext({
  Theme: {borderColor: '#1677ff'},
  AllFlowActivities:[{
    name: "HelloWordActivity1",
  },]
});

export default ApplicationContext;