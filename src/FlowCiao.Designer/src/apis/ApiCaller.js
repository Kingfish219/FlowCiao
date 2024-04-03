import axios from "axios"
const api = axios.create({
    baseURL:" http://localhost:5235/"
  })

export const ApiCaller = {
  getList: async function(resource, params, cancel = false) {
   
    const response = await api.request({
      url: `${resource}`,
      method: "GET",
      params
    })
   // var responseData = ConvertToCamelCase(response.data)
    return response
  },
  getOne: async function(resource, reqParams, cancel = false) {
    const { id, ...other } = reqParams
    const response = await api.request({
      url: `${resource}/${id}`,
      method: "GET",
      params: other.params
    })
    
   // var responseData = ConvertToCamelCase(response.data)
    return response
  },
  post: async function(resource, data, cancel = false) {
    const response = await api.request({
      url: `${resource}`,
      method: "POST",
      data: data
    })
  //  var responseData = ConvertToCamelCase(response.code == "ERR_BAD_REQUEST" ? response.response.data: response.data)
    return response
  },
  update: async function(resource, data = {}, cancel = false) {
    const { id, ...others } = data

    const response = await api.request({
      url: `${resource}${id ? "/" + id : ""}`,
      method: "PUT",
      data: others
    })

  //  var responseData = ConvertToCamelCase(response.data)
    return response
  },
  delete: async function(resource, params, cancel = false) {
    const response = await api.request({
      url: `${resource}/${params.id}`,
      method: "DELETE"
    })

   // var responseData = ConvertToCamelCase(response.data)
    return response
  },
  download: async function(resource, cancel = false) {
    const response = await api.request({
      url: `${resource}`,
      method: "GET",
      responseType: 'blob'
    })
    
    return response
  }
}

export function ConvertToCamelCase(o) {
  var newO, origKey, newKey, value
  if (o instanceof Array) {
    return o.map(function(value) {
        if (typeof value === "object") {
          value = ConvertToCamelCase(value)
        }
        return value
    })
  } else {
    newO = {}
    for (origKey in o) {
      if (o.hasOwnProperty(origKey)) {
        newKey = (origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey).toString()
        value = o[origKey]
        if (value instanceof Array || (value !== null && value.constructor === Object)) {
          value = ConvertToCamelCase(value)
        }
        newO[newKey] = value
      }
    }
  }
  return newO
}
