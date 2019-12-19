import React, {useState, useEffect} from 'react';
import Referente from './referente';

const RedReferentes = () => {

  const[dato, guardarDato]= useState([]);

    useEffect(() => {

        const api = async () => { //metodo para llamar api
        const url = 'https://localhost:44398/api/Access/GetReferentes';//guardamos la url de la api
        const res = await fetch(url);// esperamos la respuesta
        const info = await res.json();// convertimos en json
        guardarDato(info.data);

        }
       api();
       console.log([dato]);
    }, [dato])

  return (
      <div className="container cachilo pb-5">
          <div className="p-1 mb-3">
            <div className="col-md-12 info-color #33b5e5 p-1 mb-3" style={{borderRadius: "25px",marginTop:"30px"}}>
              <h5 className="h3-responsive white-text text-center mt-1">Referentes Educacion Ambiental Chaco</h5>
            </div>
          
          </div>
          <div className="row justify-content-center align-items-center card-column">
          
              {dato.map(item => (
                  
                  <div className="col-md-4 col-sm-6">

                    <Referente
                      information = {item}
                    />

                  </div>

              ))}

        </div>


      </div>
    );


}


export default RedReferentes;