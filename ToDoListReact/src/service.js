import axios from 'axios';

axios.defaults.baseURL="http://localhost:5062"

axios.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    console.log(error);
  }
);

export default {
  getTasks: async () => {
    const result = await axios.get(`${axios.defaults.baseURL}/items`)    
    return result.data;
  },
  
  addTask: async(name)=>{
    console.log('addTask', name)
    const result = await axios.post(`${axios.defaults.baseURL}/items`,{"id":0,"name":name,"isComplete":false});
    return result.data;
  },
  
  setCompleted: async(id, name, isComplete)=>{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`${axios.defaults.baseURL}/items/${id}`,{"id":id,"name":name,"isComplete":isComplete})
    return result.data;
  },
  
  deleteTask:async(id)=>{
    console.log('deleteTask')
    const result = await axios.delete(`${axios.defaults.baseURL}/items/${id}`)
    return result.data;
  }
};
