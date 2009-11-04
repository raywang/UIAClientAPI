
EXTRA_DIST =  

# Warning: This is an automatically generated file, do not edit!

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize- -debug "-define:DEBUG;TRACE"
ASSEMBLY = bin/Debug/UIAClientAPI.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug/

CONFIG_XML_SOURCE=Config.xml
NOVELL_LOGO_PNG_SOURCE=Resources/novell_logo.png
PROCEDURES_CSS_SOURCE=Resources/procedures.css
PROCEDURES_XSL_SOURCE=Resources/procedures.xsl
UIAQA_LOGO_PNG_SOURCE=Resources/uiaqa_logo.png
WHITE_CORE_DLL_SOURCE=Libs/White.Core.dll
UIACLIENTAPI_EXE_MDB_SOURCE=bin/Debug/UIAClientAPI.exe.mdb
UIACLIENTAPI_EXE_MDB=$(BUILD_DIR)/UIAClientAPI.exe.mdb

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ "-define:TRACE"
ASSEMBLY = bin/Release/UIAClientAPI.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release/

CONFIG_XML_SOURCE=Config.xml
NOVELL_LOGO_PNG_SOURCE=Resources/novell_logo.png
PROCEDURES_CSS_SOURCE=Resources/procedures.css
PROCEDURES_XSL_SOURCE=Resources/procedures.xsl
UIAQA_LOGO_PNG_SOURCE=Resources/uiaqa_logo.png
WHITE_CORE_DLL_SOURCE=Libs/White.Core.dll
UIACLIENTAPI_EXE_MDB=

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=$(notdir $(basename $(ASSEMBLY))).resources.dll

PROGRAMFILES = \
	$(CONFIG_XML) \
	$(WHITE_CORE_DLL) \
	$(UIACLIENTAPI_EXE_MDB)  

PROGRAMFILES_RESOURCES = \
	$(NOVELL_LOGO_PNG) \
	$(PROCEDURES_CSS) \
	$(PROCEDURES_XSL) \
	$(UIAQA_LOGO_PNG)  

BINARIES = \
	$(UIACLIENTAPI)  


RESGEN=resgen2
	
all: $(ASSEMBLY) $(PROGRAMFILES) $(PROGRAMFILES_RESOURCES) $(BINARIES) 

FILES = \
	Button.cs \
	CheckBox.cs \
	ComboBox.cs \
	DataGrid.cs \
	DockPatternTests.cs \
	Document.cs \
	Driver.cs \
	Edit.cs \
	Element.cs \
	List.cs \
	ListItem.cs \
	MenuBar.cs \
	MenuItem.cs \
	Pane.cs \
	RadioButton.cs \
	ScrollBar.cs \
	Spinner.cs \
	TabItem.cs \
	TestBase.cs \
	KeePassTests.cs \
	ProcedureLogger.cs \
	Properties/AssemblyInfo.cs \
	Text.cs \
	ToolBar.cs \
	Utils.cs \
	Window.cs \
	WindowPatternTests.cs 

DATA_FILES = 

RESOURCES = 

EXTRAS = \
	uiaclientapi.in 

REFERENCES =  \
	System \
	System.Core \
	System.Drawing \
	System.Windows.Forms \
	System.Xml.Linq \
	System.Data.DataSetExtensions \
	System.Data \
	System.Xml \
	$(MONO_UIA_LIBS) \
	$(MONO_UIA_DBUS_LIBS) \
	nunit.framework

DLL_REFERENCES =  \
	Libs/White.Core.dll

CLEANFILES = $(PROGRAMFILES) $(PROGRAMFILES_RESOURCES) $(BINARIES) 

include $(top_srcdir)/Makefile.include

CONFIG_XML = $(BUILD_DIR)/Config.xml
NOVELL_LOGO_PNG = $(BUILD_DIR)/Resources/novell_logo.png
PROCEDURES_CSS = $(BUILD_DIR)/Resources/procedures.css
PROCEDURES_XSL = $(BUILD_DIR)/Resources/procedures.xsl
UIAQA_LOGO_PNG = $(BUILD_DIR)/Resources/uiaqa_logo.png
WHITE_CORE_DLL = $(BUILD_DIR)/White.Core.dll
UIACLIENTAPI = $(BUILD_DIR)/uiaclientapi

$(eval $(call emit-deploy-target,CONFIG_XML))
$(eval $(call emit-deploy-target,NOVELL_LOGO_PNG))
$(eval $(call emit-deploy-target,PROCEDURES_CSS))
$(eval $(call emit-deploy-target,PROCEDURES_XSL))
$(eval $(call emit-deploy-target,UIAQA_LOGO_PNG))
$(eval $(call emit-deploy-target,WHITE_CORE_DLL))
$(eval $(call emit-deploy-wrapper,UIACLIENTAPI,uiaclientapi,x))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
