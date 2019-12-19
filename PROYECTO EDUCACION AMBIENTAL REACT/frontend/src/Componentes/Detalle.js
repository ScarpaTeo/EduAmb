import React, {useState, useEffect} from 'react';
import Detallegaleria from './DetalleGaleri'

const Detallecont = ({contenido,galeria,imagenes}) => {
    
    const {id,titulo,subtitulo,detalle,categoria,tags,name_foto} = contenido;

    let newImg2="https://localhost:44398/images/"+name_foto;



  return (
        <React.Fragment>
            

            <div class="bg-white" style={{borderRadius:"25px"}}>

                <div class="row justify-content-center align-items-center">
            <div className="col-md-11 col-sm-4 info-color #33b5e5 p-1 mb-2" style={{borderRadius: "25px",marginTop:"30px"}}>
              <h5 className="h4-responsive white-text text-center mt-1">Recorre La provincia</h5>
            </div>

                    <div class="col-12 col-md-6 mb-2 p-md-5">
                       
                        <img src={newImg2}  style={{borderRadius:"25px"}}  class="img-fluid" alt="Sample image for first version of blog listing"/>

                    </div>

                    <div class="col-12 col-md-6 my-2 p-md-5 ">


                        <h4 className="blue-text card-title mb-1 p-3" style={{fontSize:"30px",textAlign:"left"}}><strong>{titulo}</strong></h4>
                        <hr className="blue-text card-title mb-1 p-3"/>
                        <p className="gray-text card-title mb-2 p-3" style={{fontSize:"22px",textAlign:"left"}}>{subtitulo}</p>

                        <p class="card-text gray-text mb-4 p-3" style={{fontSize:"15px",textAlign:"left"}}>{detalle}</p>
                        <p class="card-text gray-text mb-4 p-3" style={{fontSize:"15px",textAlign:"left"}}>Categoria<a><strong className="blue-text" style={{fontSize:"25px"}}> {tags} </strong></a></p>

                    

                    </div>
                    <hr width="80%" style={{color:"blue", borderStyle:"solid"}}/>

                    <div className="col-12 col-md-10">
                    <p class="card-text text-center gray-text mb-4 p-3" style={{fontSize:"15px",textAlign:"left"}}>Galeria de Multimedia</p>
                    
                  
                        
                            <div className="row">
                            {imagenes.map(items=>(
                                <div className="col-md-3 my-1">

                                    <Detallegaleria
                                    galeria={items}
                                    /> 

                                </div>
                            ))}
                        </div>
                    </div>

                </div>

            </div>




        </React.Fragment>
    )
}
export default Detallecont;