using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VeracodeService.Models;
using VeraData.DataAccess;
using VeraData.DataAccess.Models;

namespace VeraData
{
    public interface IMappingService
    {
        Application[] Apps(AppType[] entities);
        Scan[] Scans(BuildInfoBuildType[] builds);
        Flaw[] Flaws(FlawType[] entities);
        Module[] Modules(ModuleType[] entities);
        Sandbox[] Sandboxes(SandboxType[] entities);
        SourceFile[] SourceFiles(FlawType[] entities);
        UploadFile[] UploadFiles(FileListFileType[] entities);
        Category[] Cetegories(FlawType[] entities);
        Cwe[] Cwe(FlawType[] entities);
        MitigationAction[] Mitigations(MitigationActionType[] entities);
    }

    public class MappingService : IMappingService
    {
        private readonly IMapper _mapper;
        public MappingService(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Application[] Apps(AppType[] entities) => entities.Select(x => _mapper.Map<Application>(x)).ToArray();
        public Scan[] Scans(BuildInfoBuildType[] entities) => entities.Select(x => _mapper.Map<Scan>(x)).ToArray();
        public Flaw[] Flaws(FlawType[] entities) => entities.Select(x => _mapper.Map<Flaw>(x)).ToArray();
        public Module[] Modules(ModuleType[] entities) => entities.Select(x => _mapper.Map<Module>(x)).ToArray();
        public Sandbox[] Sandboxes(SandboxType[] entities) => entities.Select(x => _mapper.Map<Sandbox>(x)).ToArray();
        public SourceFile[] SourceFiles(FlawType[] entities) => entities.Select(x => _mapper.Map<SourceFile>(x)).ToArray();
        public UploadFile[] UploadFiles(FileListFileType[] entities) => entities.Select(x => _mapper.Map<UploadFile>(x)).ToArray();
        public Category[] Cetegories(FlawType[] entities) => entities.Select(x => _mapper.Map<Category>(x)).ToArray();
        public Cwe[] Cwe(FlawType[] entities) => entities.Select(x => _mapper.Map<Cwe>(x)).ToArray();
        public MitigationAction[] Mitigations(MitigationActionType[] entities) => entities.Select(x => _mapper.Map<MitigationAction>(x)).ToArray();
    }
}
