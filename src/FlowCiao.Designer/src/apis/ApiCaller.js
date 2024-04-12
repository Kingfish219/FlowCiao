import axios from "axios";
const api = axios.create({
  baseURL: process.env.REACT_APP_BASE_API_URL,
});
export const ApiCaller = {
  getList: async function (resource, params, cancel = false) {
    try {
      const response = await api.request({
        url: `${resource}`,
        method: "GET",
        params,
      });
      return response;
    } catch (err) {
      return err.response;
    }
  },
  getOne: async function (resource, reqParams, cancel = false) {
    try {
      const { id, ...other } = reqParams;
      const response = await api.request({
        url: `${resource}/${id}`,
        method: "GET",
        params: other.params,
      });
      return response;
    } catch (err) {
      return err.response;
    }
  },
  post: async function (resource, data, cancel = false) {
    try {
      const response = await api.request({
        url: `${resource}`,
        method: "POST",
        data: data,
      });
      return response;
    } catch (err) {
      return err.response;
    }
  },
  update: async function (resource, data = {}, cancel = false) {
    try {
      const { id, ...others } = data;

      const response = await api.request({
        url: `${resource}${id ? "/" + id : ""}`,
        method: "PUT",
        data: others,
      });
      return response;
    } catch (err) {
      return err.response;
    }
  },
  delete: async function (resource, params, cancel = false) {
    try {
      const response = await api.request({
        url: `${resource}/${params.id}`,
        method: "DELETE",
      });
      return response;
    } catch (err) {
      return err.response;
    }
  },
  download: async function (resource, cancel = false) {
    const response = await api.request({
      url: `${resource}`,
      method: "GET",
      responseType: "blob",
    });

    return response;
  },
};

export function ConvertToCamelCase(o) {
  var newO, origKey, newKey, value;
  if (o instanceof Array) {
    return o.map(function (value) {
      if (typeof value === "object") {
        value = ConvertToCamelCase(value);
      }
      return value;
    });
  } else {
    newO = {};
    for (origKey in o) {
      if (o.hasOwnProperty(origKey)) {
        newKey = (
          origKey.charAt(0).toLowerCase() + origKey.slice(1) || origKey
        ).toString();
        value = o[origKey];
        if (
          value instanceof Array ||
          (value !== null && value.constructor === Object)
        ) {
          value = ConvertToCamelCase(value);
        }
        newO[newKey] = value;
      }
    }
  }
  return newO;
}
