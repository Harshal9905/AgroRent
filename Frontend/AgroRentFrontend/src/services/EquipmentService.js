import axios from "axios";
import { getToken } from "./UserServices";

export function getAllEquipment() {
    const token = getToken();
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };
    
    return axios.get("http://localhost:9090/equipment", config);
}

export function getSingleEquipment(equipmentId) {
    const token = getToken();
    const config = {
        headers: {
            'Authorization': `Bearer ${token}`
        }
    };
    
    return axios.get(`http://localhost:9090/equipment/${equipmentId}`, config);
} 