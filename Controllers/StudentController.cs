using CollegeApp_API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;
using CollegeApp_API.Data;
using AutoMapper;
using CollegeApp_API.Data.Interfaces;
using CollegeApp_API.Models;
using System.Net;

namespace CollegeApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IStudentRepository _studentRepository;
        private CommonApiResponse _commonApiResponse;

        public StudentController(IMapper mapper, IStudentRepository studentRepository)
        {
            _mapper = mapper;
            _studentRepository = studentRepository;
            _commonApiResponse = new CommonApiResponse();
        }

        [HttpGet]
        [Route("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommonApiResponse>> GetStudentsAsync()
        {
            try
            {
                var students = await _studentRepository.GetAllAsync();

                _commonApiResponse.Data = _mapper.Map<List<StudentDTO>>(students);
                _commonApiResponse.Success = true;
                _commonApiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_commonApiResponse);
            }
            catch (Exception ex)
            {
                _commonApiResponse.Success = false;
                _commonApiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _commonApiResponse.Errors.Add(ex.Message);
                return _commonApiResponse;
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "GetStudentById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommonApiResponse>> GetStudentByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                if (student == null)
                {
                    return NotFound($"The student with {id} not found.");
                }

                _commonApiResponse.Data = _mapper.Map<StudentDTO>(student);
                _commonApiResponse.Success = true;
                _commonApiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_commonApiResponse);
            }
            catch (Exception ex)
            {
                _commonApiResponse.Success = false;
                _commonApiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _commonApiResponse.Errors.Add(ex.Message);
                return _commonApiResponse;
            }
        }

        [HttpGet]
        [Route("{name:alpha}", Name = "GetStudentByName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommonApiResponse>> GetStudentByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetAsync(student => student.Name.ToLower().Contains(name.ToLower()));
                if (student == null)
                {
                    return NotFound($"The student with {name} not found.");
                }

                _commonApiResponse.Data = _mapper.Map<StudentDTO>(student);
                _commonApiResponse.Success = true;
                _commonApiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_commonApiResponse);
            }
            catch (Exception ex)
            {
                _commonApiResponse.Success = false;
                _commonApiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _commonApiResponse.Errors.Add(ex.Message);
                return _commonApiResponse;
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommonApiResponse>> DeleteStudentAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest();
                }

                var student = await _studentRepository.GetAsync(student => student.Id == id);
                if (student == null)
                    return NotFound($"The student with {id} not found.");

                await _studentRepository.DeleteAsync(student);

                _commonApiResponse.Data = true;
                _commonApiResponse.Success = true;
                _commonApiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_commonApiResponse);
            }
            catch (Exception ex)
            {
                _commonApiResponse.Success = false;
                _commonApiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _commonApiResponse.Errors.Add(ex.Message);
                return _commonApiResponse;
            }
        }

        [HttpPost]
        [Route("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CommonApiResponse>> CreateStudentAsync([FromBody] StudentDTO studentModel)
        {
            try
            {
                if (studentModel == null)
                    return BadRequest();

                Student student = _mapper.Map<Student>(studentModel);

                var studentAfterCreation = await _studentRepository.CreateAsync(student);

                studentModel.Id = studentAfterCreation.Id;

                _commonApiResponse.Data = studentModel;
                _commonApiResponse.Success = true;
                _commonApiResponse.StatusCode = HttpStatusCode.OK;

                return CreatedAtRoute("GetStudentById", new { id = studentModel.Id }, _commonApiResponse);
            }
            catch (Exception ex)
            {
                _commonApiResponse.Success = false;
                _commonApiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _commonApiResponse.Errors.Add(ex.Message);
                return _commonApiResponse;
            }
        }

        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentAsync([FromBody] StudentDTO studentModel)
        {
            if (studentModel == null || studentModel.Id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == studentModel.Id, true);
            if (existingStudent == null)
                return NotFound();

            var newRecord = _mapper.Map<Student>(studentModel);

            await _studentRepository.UpdateAsync(newRecord);

            return NoContent();
        }

        [HttpPut("{id:int}/updatePartial")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateStudentPartialAsync(int id, [FromBody] JsonPatchDocument<StudentDTO> patchDocument)
        {
            if (patchDocument == null || id <= 0)
            {
                return BadRequest();
            }

            var existingStudent = await _studentRepository.GetAsync(student => student.Id == id, true);
            if (existingStudent == null)
                return NotFound();

            var studentDto = _mapper.Map<StudentDTO>(existingStudent);

            patchDocument.ApplyTo(studentDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            existingStudent = _mapper.Map<Student>(studentDto);

            await _studentRepository.UpdateAsync(existingStudent);

            return NoContent();
        }
    }
}
