﻿CREATE PROC FI_SP_AltBeneficiario
    @NOME          VARCHAR (50) ,
    @CPF           VARCHAR (11)  ,
	@IDCLIENTE     BIGINT       ,
	@Id            BIGINT
AS
BEGIN
	UPDATE BENEFICIARIOS
	SET 
		CPF = @CPF, 
		NOME = @NOME, 
		IDCLIENTE = IDCLIENTE
	WHERE ID = @Id
END